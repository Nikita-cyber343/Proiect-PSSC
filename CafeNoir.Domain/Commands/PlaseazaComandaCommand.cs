using System;
using System.Collections.Generic;

namespace CafeNoir.Domain.Commands
{
    public record PlaseazaComandaCommand
    {
        public string NumeClient { get; init; }
        public string Email { get; init; }
        public string AdresaLivrare { get; init; }
        public List<LinieComanda> Produse { get; init; }

        public record LinieComanda
        {
            public Guid ProdusId { get; init; }
            public int Cantitate { get; init; }
        }
    }
}