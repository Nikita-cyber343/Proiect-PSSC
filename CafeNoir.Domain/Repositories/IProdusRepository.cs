using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CafeNoir.Domain.Entities;
using CafeNoir.Domain.Models;

namespace CafeNoir.Domain.Repositories
{
    public interface IProdusRepository
    {
        Task<Produs> GetByIdAsync(ProdusId id);
        Task<List<Produs>> GetByIdsAsync(List<ProdusId> ids);
        Task UpdateAsync(Produs produs);
    }
}