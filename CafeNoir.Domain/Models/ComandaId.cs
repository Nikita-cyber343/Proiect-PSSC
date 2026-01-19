using System;

namespace CafeNoir.Domain.Models
{
    public record ComandaId
    {
        public Guid Value { get; }

        private ComandaId(Guid value)
        {
            Value = value;
        }

        public static ComandaId Create(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id-ul comenzii nu poate fi gol");

            return new ComandaId(id);
        }

        public static ComandaId CreateNew()
        {
            return new ComandaId(Guid.NewGuid());
        }
    }
}