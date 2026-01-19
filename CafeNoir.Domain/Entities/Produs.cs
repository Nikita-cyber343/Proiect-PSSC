using CafeNoir.Domain.Models;

namespace CafeNoir.Domain.Entities
{
    public class Produs
    {
        public ProdusId Id { get; set; }
        public NumeProdus Nume { get; set; }
        public Pret Pret { get; set; }
        public int StocDisponibil { get; set; }

        public bool VerificaStoc(Cantitate cantitate)
        {
            return StocDisponibil >= cantitate.Value;
        }

        public void ScadeStoc(Cantitate cantitate)
        {
            StocDisponibil -= cantitate.Value;
        }
    }
}