using System;

namespace CafeNoir.Domain.Events
{
    public record ComandaAnulataEvent
    {
        public Guid ComandaId { get; init; }
        public string Motiv { get; init; }
        public DateTime DataAnularii { get; init; }
    }
}