using System;

namespace CafeNoir.Domain.Events
{
    public record ComandaExpediataEvent
    {
        public Guid ComandaId { get; init; }
        public string NumarAwb { get; init; }
        public string Curier { get; init; }
        public DateTime DataExpedierii { get; init; }
    }
}