using System;

namespace CafeNoir.Domain.Models
{
    public record NumeClient
    {
        public string Value { get; }

        private NumeClient(string value)
        {
            Value = value;
        }

        public static NumeClient Create(string nume)
        {
            if (string.IsNullOrWhiteSpace(nume))
                throw new ArgumentException("Numele nu poate fi gol");

            if (nume.Length < 2)
                throw new ArgumentException("Numele trebuie să aibă cel puțin 2 caractere");

            return new NumeClient(nume);
        }
    }
}