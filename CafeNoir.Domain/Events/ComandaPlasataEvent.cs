using System;

namespace CafeNoir.Domain.Events
{
    public record ComandaPlasataEvent
    {
        public Guid ComandaId { get; init; }
        public string NumeClient { get; init; }
        public string Email { get; init; }
        public decimal PretTotal { get; init; }
        public DateTime DataPlasarii { get; init; }
    }
}