using CafeNoir.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeNoir.Infrastructure.Data
{
    public class CafeNoirDbContext : DbContext
    {
        public CafeNoirDbContext(DbContextOptions<CafeNoirDbContext> options)
            : base(options)
        {
        }

        public DbSet<ComandaEntity> Comenzi { get; set; }
        public DbSet<ProdusEntity> Produse { get; set; }
        public DbSet<LinieComandaEntity> LiniiComanda { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurare tabel Comenzi
            modelBuilder.Entity<ComandaEntity>(entity =>
            {
                entity.ToTable("Comenzi");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeClient).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.AdresaLivrare).IsRequired().HasMaxLength(500);
                entity.Property(e => e.PretTotal).HasPrecision(18, 2);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            });

            // Configurare tabel Produse
            modelBuilder.Entity<ProdusEntity>(entity =>
            {
                entity.ToTable("Produse");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nume).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Pret).HasPrecision(18, 2);
            });

            // Configurare tabel LiniiComanda
            modelBuilder.Entity<LinieComandaEntity>(entity =>
            {
                entity.ToTable("LiniiComanda");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeProdus).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PretUnitar).HasPrecision(18, 2);

                entity.HasOne(e => e.Comanda)
                    .WithMany(c => c.Linii)
                    .HasForeignKey(e => e.ComandaId);

                entity.HasOne(e => e.Produs)
                    .WithMany()
                    .HasForeignKey(e => e.ProdusId);
            });

            // Seed data - produse inițiale
            modelBuilder.Entity<ProdusEntity>().HasData(
                new ProdusEntity
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Nume = "Espresso",
                    Pret = 8.50m,
                    StocDisponibil = 100
                },
                new ProdusEntity
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Nume = "Cappuccino",
                    Pret = 12.00m,
                    StocDisponibil = 100
                },
                new ProdusEntity
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Nume = "Latte",
                    Pret = 13.50m,
                    StocDisponibil = 100
                },
                new ProdusEntity
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Nume = "Croissant",
                    Pret = 7.00m,
                    StocDisponibil = 50
                }
            );
        }
    }
}