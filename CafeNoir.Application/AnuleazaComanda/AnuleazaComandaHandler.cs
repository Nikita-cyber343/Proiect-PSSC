using System;
using System.Threading.Tasks;
using CafeNoir.Domain.Commands;
using CafeNoir.Domain.Common;
using CafeNoir.Domain.Events;
using CafeNoir.Domain.Models;
using CafeNoir.Domain.Repositories;
using MassTransit;

namespace CafeNoir.Application.AnuleazaComanda
{
    public class AnuleazaComandaHandler
    {
        private readonly IComandaRepository _comandaRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public AnuleazaComandaHandler(
            IComandaRepository comandaRepository,
            IPublishEndpoint publishEndpoint)
        {
            _comandaRepository = comandaRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result> Handle(AnuleazaComandaCommand command)
        {
            try
            {
                // PASUL 1: Găsește comanda
                var comandaId = ComandaId.Create(command.ComandaId);
                var comanda = await _comandaRepository.GetByIdAsync(comandaId);

                if (comanda == null)
                    return Result.Failure("Comanda nu există");

                // PASUL 2: Anulează comanda
                try
                {
                    comanda.Anuleaza();
                }
                catch (InvalidOperationException ex)
                {
                    return Result.Failure(ex.Message);
                }

                // PASUL 3: Salvează modificările
                await _comandaRepository.UpdateAsync(comanda);

                // PASUL 4: Publică event
                var comandaAnulataEvent = new ComandaAnulataEvent
                {
                    ComandaId = comandaId.Value,
                    Motiv = command.Motiv,
                    DataAnularii = DateTime.UtcNow
                };

                await _publishEndpoint.Publish(comandaAnulataEvent);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Eroare la anularea comenzii: {ex.Message}");
            }
        }
    }
}