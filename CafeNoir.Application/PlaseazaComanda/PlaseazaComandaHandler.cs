using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeNoir.Domain.Commands;
using CafeNoir.Domain.Common;
using CafeNoir.Domain.Entities;
using CafeNoir.Domain.Events;
using CafeNoir.Domain.Models;
using CafeNoir.Domain.Repositories;
using MassTransit;

namespace CafeNoir.Application.PlaseazaComanda
{
    public class PlaseazaComandaHandler
    {
        private readonly IComandaRepository _comandaRepository;
        private readonly IProdusRepository _produsRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public PlaseazaComandaHandler(
            IComandaRepository comandaRepository,
            IProdusRepository produsRepository,
            IPublishEndpoint publishEndpoint)
        {
            _comandaRepository = comandaRepository;
            _produsRepository = produsRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<Guid>> Handle(PlaseazaComandaCommand command)
        {
            try
            {
                // PASUL 1: Validare date client
                var numeClient = NumeClient.Create(command.NumeClient);
                var email = Email.Create(command.Email);
                var adresa = AdresaLivrare.Create(command.AdresaLivrare);

                // PASUL 2: Verificare produse și stoc
                var produseIds = command.Produse.Select(p => ProdusId.Create(p.ProdusId)).ToList();
                var produse = await _produsRepository.GetByIdsAsync(produseIds);

                if (produse.Count != command.Produse.Count)
                    return Result<Guid>.Failure("Unele produse nu există");

                var liniiComanda = new List<LinieComanda>();
                var pretTotal = Pret.Create(0);

                foreach (var linie in command.Produse)
                {
                    var produs = produse.First(p => p.Id.Value == linie.ProdusId);
                    var cantitate = Cantitate.Create(linie.Cantitate);

                    // Verifică stoc
                    if (!produs.VerificaStoc(cantitate))
                        return Result<Guid>.Failure($"Stoc insuficient pentru {produs.Nume.Value}");

                    // Creează linie comandă
                    var linieComanda = new LinieComanda
                    {
                        ProdusId = produs.Id,
                        NumeProdus = produs.Nume,
                        Cantitate = cantitate,
                        PretUnitar = produs.Pret
                    };

                    liniiComanda.Add(linieComanda);
                    pretTotal = pretTotal + linieComanda.CalculeazaPretTotal();

                    // Scade stoc
                    produs.ScadeStoc(cantitate);
                    await _produsRepository.UpdateAsync(produs);
                }

                // PASUL 3: Creează comanda
                var comandaId = ComandaId.CreateNew();
                var comanda = new Comanda(
                    comandaId,
                    numeClient,
                    email,
                    adresa,
                    pretTotal,
                    liniiComanda
                );

                // PASUL 4: Salvează comanda
                await _comandaRepository.SaveAsync(comanda);

                // PASUL 5: Publică event
                var comandaPlasataEvent = new ComandaPlasataEvent
                {
                    ComandaId = comandaId.Value,
                    NumeClient = numeClient.Value,
                    Email = email.Value,
                    PretTotal = pretTotal.Value,
                    DataPlasarii = DateTime.UtcNow
                };

                await _publishEndpoint.Publish(comandaPlasataEvent);

                return Result<Guid>.Success(comandaId.Value);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Eroare la plasarea comenzii: {ex.Message}");
            }
        }
    }
}