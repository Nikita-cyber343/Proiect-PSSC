using System;

namespace CafeNoir.Domain.Commands
{
    public record ExpediazaComandaCommand
    {
        public Guid ComandaId { get; init; }
        public string NumarAwb { get; init; }
        public string Curier { get; init; }
    }
}