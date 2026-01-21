using System;
using System.Threading.Tasks;
using CafeNoir.Domain.Entities;
using CafeNoir.Domain.Models;

namespace CafeNoir.Domain.Repositories
{
    public interface IComandaRepository
    {
        Task<Comanda> GetByIdAsync(ComandaId id);
        Task<Comanda> SaveAsync(Comanda comanda);
        Task UpdateAsync(Comanda comanda);
    }
}