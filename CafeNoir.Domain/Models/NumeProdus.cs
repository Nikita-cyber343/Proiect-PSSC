using System;

namespace CafeNoir.Domain.Models
{
    public record NumeProdus
    {
        public string Value { get; }

        private NumeProdus(string value)
        {
            Value = value;
        }

        public static NumeProdus Create(string nume)
        {
            if (string.IsNullOrWhiteSpace(nume))
                throw new ArgumentException("Numele produsului nu poate fi gol");

            return new NumeProdus(nume);
        }
    }
}