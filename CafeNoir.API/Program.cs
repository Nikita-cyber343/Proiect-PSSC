using CafeNoir.Application.PlaseazaComanda;
using CafeNoir.Application.AnuleazaComanda;
using CafeNoir.Application.ExpediazaComanda;
using CafeNoir.Domain.Repositories;
using CafeNoir.Infrastructure.Data;
using CafeNoir.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CafeNoirDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CafeNoir.API")
    )
);

builder.Services.AddScoped<IComandaRepository, ComandaRepository>();
builder.Services.AddScoped<IProdusRepository, ProdusRepository>();

builder.Services.AddScoped<PlaseazaComandaHandler>();
builder.Services.AddScoped<AnuleazaComandaHandler>();
builder.Services.AddScoped<ExpediazaComandaHandler>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ComandaPlasataConsumer>();

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CafeNoirDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();