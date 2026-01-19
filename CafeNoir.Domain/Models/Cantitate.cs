using System;

namespace CafeNoir.Domain.Models
{
    public record Cantitate
    {
        public int Value { get; }

        private Cantitate(int value)
        {
            Value = value;
        }

        public static Cantitate Create(int cantitate)
        {
            if (cantitate <= 0)
                throw new ArgumentException("Cantitatea trebuie să fie pozitivă");

            if (cantitate > 1000)
                throw new ArgumentException("Cantitatea este prea mare");

            return new Cantitate(cantitate);
        }
    }
}