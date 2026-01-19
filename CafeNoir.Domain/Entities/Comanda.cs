using System;
using System.Collections.Generic;
using CafeNoir.Domain.Models;

namespace CafeNoir.Domain.Entities
{
    public class Comanda
    {
        public ComandaId Id { get; private set; }
        public NumeClient NumeClient { get; private set; }
        public Email Email { get; private set; }
        public AdresaLivrare AdresaLivrare { get; private set; }
        public Pret PretTotal { get; private set; }
        public StatusComanda Status { get; private set; }
        public DateTime DataPlasarii { get; private set; }
        public List<LinieComanda> Linii { get; private set; }

        public Comanda(
            ComandaId id,
            NumeClient numeClient,
            Email email,
            AdresaLivrare adresaLivrare,
            Pret pretTotal,
            List<LinieComanda> linii)
        {
            Id = id;
            NumeClient = numeClient;
            Email = email;
            AdresaLivrare = adresaLivrare;
            PretTotal = pretTotal;
            Status = StatusComanda.Plasata;
            DataPlasarii = DateTime.UtcNow;
            Linii = linii;
        }

        public void Anuleaza()
        {
            if (Status == StatusComanda.Expediata)
                throw new InvalidOperationException("Nu se poate anula o comandă deja expediată");

            Status = StatusComanda.Anulata;
        }

        public void Expedieaza()
        {
            if (Status != StatusComanda.Plasata)
                throw new InvalidOperationException("Doar comenzile plasate pot fi expediate");

            Status = StatusComanda.Expediata;
        }
    }

    public class LinieComanda
    {
        public ProdusId ProdusId { get; set; }
        public NumeProdus NumeProdus { get; set; }
        public Cantitate Cantitate { get; set; }
        public Pret PretUnitar { get; set; }

        public Pret CalculeazaPretTotal()
        {
            return PretUnitar * Cantitate.Value;
        }
    }

    public enum StatusComanda
    {
        Plasata,
        Anulata,
        Expediata
    }
}