using System;

namespace CafeNoir.Domain.Commands
{
    public record AnuleazaComandaCommand
    {
        public Guid ComandaId { get; init; }
        public string Motiv { get; init; }
    }
}