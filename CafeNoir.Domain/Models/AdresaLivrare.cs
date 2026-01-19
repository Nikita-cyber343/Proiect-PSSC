using System;

namespace CafeNoir.Domain.Models
{
    public record AdresaLivrare
    {
        public string Value { get; }

        private AdresaLivrare(string value)
        {
            Value = value;
        }

        public static AdresaLivrare Create(string adresa)
        {
            if (string.IsNullOrWhiteSpace(adresa))
                throw new ArgumentException("Adresa nu poate fi goală");

            if (adresa.Length < 10)
                throw new ArgumentException("Adresa trebuie să aibă cel puțin 10 caractere");

            return new AdresaLivrare(adresa);
        }
    }
}