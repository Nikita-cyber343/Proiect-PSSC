using System;
using System.Threading.Tasks;
using CafeNoir.Domain.Commands;
using CafeNoir.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CafeNoir.Application.ExpediazaComanda
{
    // Acest consumer ASCULTĂ event-ul ComandaPlasataEvent
    // și AUTOMAT pornește procesul de expediere
    public class ComandaPlasataConsumer : IConsumer<ComandaPlasataEvent>
    {
        private readonly ExpediazaComandaHandler _handler;
        private readonly ILogger<ComandaPlasataConsumer> _logger;

        public ComandaPlasataConsumer(
            ExpediazaComandaHandler handler,
            ILogger<ComandaPlasataConsumer> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ComandaPlasataEvent> context)
        {
            _logger.LogInformation($"S-a primit event ComandaPlasata pentru comanda {context.Message.ComandaId}");

            // Așteaptă 2 secunde (simulare pregătire comandă)
            await Task.Delay(2000);

            // Expediază automat comanda
            var command = new ExpediazaComandaCommand
            {
                ComandaId = context.Message.ComandaId,
                NumarAwb = $"AWB{DateTime.Now.Ticks}",
                Curier = "FanCourier"
            };

            var result = await _handler.Handle(command);

            if (result.IsSuccess)
                _logger.LogInformation($"Comanda {context.Message.ComandaId} a fost expediată cu succes");
            else
                _logger.LogError($"Eroare la expedierea comenzii {context.Message.ComandaId}: {result.Error}");
        }
    }
}