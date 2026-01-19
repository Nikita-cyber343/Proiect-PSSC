using System;

namespace CafeNoir.Domain.Models
{
    public record Pret
    {
        public decimal Value { get; }

        private Pret(decimal value)
        {
            Value = value;
        }

        public static Pret Create(decimal pret)
        {
            if (pret < 0)
                throw new ArgumentException("Prețul nu poate fi negativ");

            if (pret > 100000)
                throw new ArgumentException("Prețul este prea mare");

            return new Pret(pret);
        }

        public static Pret operator +(Pret a, Pret b)
        {
            return new Pret(a.Value + b.Value);
        }

        public static Pret operator *(Pret pret, int cantitate)
        {
            return new Pret(pret.Value * cantitate);
        }
    }
}