using System;
using System.Linq;
using System.Threading.Tasks;
using CafeNoir.Domain.Entities;
using CafeNoir.Domain.Models;
using CafeNoir.Domain.Repositories;
using CafeNoir.Infrastructure.Data;
using CafeNoir.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeNoir.Infrastructure.Repositories
{
    public class ComandaRepository : IComandaRepository
    {
        private readonly CafeNoirDbContext _context;

        public ComandaRepository(CafeNoirDbContext context)
        {
            _context = context;
        }

        public async Task<Comanda> GetByIdAsync(ComandaId id)
        {
            var entity = await _context.Comenzi
                .Include(c => c.Linii)
                .FirstOrDefaultAsync(c => c.Id == id.Value);

            if (entity == null)
                return null;

            return MapToDomain(entity);
        }

        public async Task<Comanda> SaveAsync(Comanda comanda)
        {
            var entity = MapToEntity(comanda);
            await _context.Comenzi.AddAsync(entity);
            await _context.SaveChangesAsync();
            return comanda;
        }

        public async Task UpdateAsync(Comanda comanda)
        {
            var entity = await _context.Comenzi.FindAsync(comanda.Id.Value);
            if (entity != null)
            {
                entity.Status = comanda.Status.ToString();
                await _context.SaveChangesAsync();
            }
        }

        private Comanda MapToDomain(ComandaEntity entity)
        {
            var linii = entity.Linii.Select(l => new LinieComanda
            {
                ProdusId = ProdusId.Create(l.ProdusId),
                NumeProdus = NumeProdus.Create(l.NumeProdus),
                Cantitate = Cantitate.Create(l.Cantitate),
                PretUnitar = Pret.Create(l.PretUnitar)
            }).ToList();

            return new Comanda(
                ComandaId.Create(entity.Id),
                NumeClient.Create(entity.NumeClient),
                Email.Create(entity.Email),
                AdresaLivrare.Create(entity.AdresaLivrare),
                Pret.Create(entity.PretTotal),
                linii
            );
        }

        private ComandaEntity MapToEntity(Comanda comanda)
        {
            return new ComandaEntity
            {
                Id = comanda.Id.Value,
                NumeClient = comanda.NumeClient.Value,
                Email = comanda.Email.Value,
                AdresaLivrare = comanda.AdresaLivrare.Value,
                PretTotal = comanda.PretTotal.Value,
                Status = comanda.Status.ToString(),
                DataPlasarii = comanda.DataPlasarii,
                Linii = comanda.Linii.Select(l => new LinieComandaEntity
                {
                    Id = Guid.NewGuid(),
                    ProdusId = l.ProdusId.Value,
                    NumeProdus = l.NumeProdus.Value,
                    Cantitate = l.Cantitate.Value,
                    PretUnitar = l.PretUnitar.Value
                }).ToList()
            };
        }
    }
}