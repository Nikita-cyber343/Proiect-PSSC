using System;
using System.Threading.Tasks;
using CafeNoir.Domain.Commands;
using CafeNoir.Domain.Common;
using CafeNoir.Domain.Events;
using CafeNoir.Domain.Models;
using CafeNoir.Domain.Repositories;
using MassTransit;

namespace CafeNoir.Application.ExpediazaComanda
{
    public class ExpediazaComandaHandler
    {
        private readonly IComandaRepository _comandaRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ExpediazaComandaHandler(
            IComandaRepository comandaRepository,
            IPublishEndpoint publishEndpoint)
        {
            _comandaRepository = comandaRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result> Handle(ExpediazaComandaCommand command)
        {
            try
            {
                // PASUL 1: Găsește comanda
                var comandaId = ComandaId.Create(command.ComandaId);
                var comanda = await _comandaRepository.GetByIdAsync(comandaId);

                if (comanda == null)
                    return Result.Failure("Comanda nu există");

                // PASUL 2: Expediază comanda
                try
                {
                    comanda.Expedieaza();
                }
                catch (InvalidOperationException ex)
                {
                    return Result.Failure(ex.Message);
                }

                // PASUL 3: Salvează modificările
                await _comandaRepository.UpdateAsync(comanda);

                // PASUL 4: Publică event
                var comandaExpediataEvent = new ComandaExpediataEvent
                {
                    ComandaId = comandaId.Value,
                    NumarAwb = command.NumarAwb,
                    Curier = command.Curier,
                    DataExpedierii = DateTime.UtcNow
                };

                await _publishEndpoint.Publish(comandaExpediataEvent);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Eroare la expedierea comenzii: {ex.Message}");
            }
        }
    }
}