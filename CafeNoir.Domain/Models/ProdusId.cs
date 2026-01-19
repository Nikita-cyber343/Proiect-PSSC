using System;

namespace CafeNoir.Domain.Models
{
    public record ProdusId
    {
        public Guid Value { get; }

        private ProdusId(Guid value)
        {
            Value = value;
        }

        public static ProdusId Create(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id-ul produsului nu poate fi gol");

            return new ProdusId(id);
        }

        public static ProdusId CreateNew()
        {
            return new ProdusId(Guid.NewGuid());
        }
    }
}