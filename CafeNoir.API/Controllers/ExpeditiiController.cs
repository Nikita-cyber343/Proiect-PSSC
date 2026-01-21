using System;
using System.Threading.Tasks;
using CafeNoir.Application.ExpediazaComanda;
using CafeNoir.Domain.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CafeNoir.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpeditiiController : ControllerBase
    {
        private readonly ExpediazaComandaHandler _handler;

        public ExpeditiiController(ExpediazaComandaHandler handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// Expediază manual o comandă (în caz că vrei să forțezi expedierea)
        /// </summary>
        [HttpPost("{id}/expediere")]
        public async Task<IActionResult> ExpediazaComanda(Guid id, [FromBody] ExpeditieRequest request)
        {
            var command = new ExpediazaComandaCommand
            {
                ComandaId = id,
                NumarAwb = request.NumarAwb ?? $"AWB{DateTime.Now.Ticks}",
                Curier = request.Curier ?? "FanCourier"
            };

            var result = await _handler.Handle(command);

            if (result.IsSuccess)
                return Ok(new { message = "Comanda a fost expediată cu succes!" });

            return BadRequest(new { error = result.Error });
        }
    }

    public class ExpeditieRequest
    {
        public string NumarAwb { get; set; }
        public string Curier { get; set; }
    }
}