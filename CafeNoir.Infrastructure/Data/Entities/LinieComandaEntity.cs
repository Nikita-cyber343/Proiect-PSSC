using System;

namespace CafeNoir.Infrastructure.Data.Entities
{
    public class LinieComandaEntity
    {
        public Guid Id { get; set; }
        public Guid ComandaId { get; set; }
        public Guid ProdusId { get; set; }
        public string NumeProdus { get; set; }
        public int Cantitate { get; set; }
        public decimal PretUnitar { get; set; }

        public ComandaEntity Comanda { get; set; }
        public ProdusEntity Produs { get; set; }
    }
}