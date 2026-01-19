using System;
using System.Collections.Generic;

namespace CafeNoir.Infrastructure.Data.Entities
{
    public class ComandaEntity
    {
        public Guid Id { get; set; }
        public string NumeClient { get; set; }
        public string Email { get; set; }
        public string AdresaLivrare { get; set; }
        public decimal PretTotal { get; set; }
        public string Status { get; set; }
        public DateTime DataPlasarii { get; set; }

        public List<LinieComandaEntity> Linii { get; set; }
    }
}