using System;

namespace CafeNoir.Infrastructure.Data.Entities
{
    public class ProdusEntity
    {
        public Guid Id { get; set; }
        public string Nume { get; set; }
        public decimal Pret { get; set; }
        public int StocDisponibil { get; set; }
    }
}