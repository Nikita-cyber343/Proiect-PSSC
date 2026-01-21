using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeNoir.Domain.Entities;
using CafeNoir.Domain.Models;
using CafeNoir.Domain.Repositories;
using CafeNoir.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CafeNoir.Infrastructure.Repositories
{
    public class ProdusRepository : IProdusRepository
    {
        private readonly CafeNoirDbContext _context;

        public ProdusRepository(CafeNoirDbContext context)
        {
            _context = context;
        }

        public async Task<Produs> GetByIdAsync(ProdusId id)
        {
            var entity = await _context.Produse.FindAsync(id.Value);
            if (entity == null)
                return null;

            return new Produs
            {
                Id = ProdusId.Create(entity.Id),
                Nume = NumeProdus.Create(entity.Nume),
                Pret = Pret.Create(entity.Pret),
                StocDisponibil = entity.StocDisponibil
            };
        }

        public async Task<List<Produs>> GetByIdsAsync(List<ProdusId> ids)
        {
            var guids = ids.Select(id => id.Value).ToList();
            var entities = await _context.Produse
                .Where(p => guids.Contains(p.Id))
                .ToListAsync();

            return entities.Select(e => new Produs
            {
                Id = ProdusId.Create(e.Id),
                Nume = NumeProdus.Create(e.Nume),
                Pret = Pret.Create(e.Pret),
                StocDisponibil = e.StocDisponibil
            }).ToList();
        }

        public async Task UpdateAsync(Produs produs)
        {
            var entity = await _context.Produse.FindAsync(produs.Id.Value);
            if (entity != null)
            {
                entity.StocDisponibil = produs.StocDisponibil;
                await _context.SaveChangesAsync();
            }
        }
    }
}