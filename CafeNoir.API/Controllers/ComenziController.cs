using System;
using System.Threading.Tasks;
using CafeNoir.Application.PlaseazaComanda;
using CafeNoir.Application.AnuleazaComanda;
using CafeNoir.Domain.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CafeNoir.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComenziController : ControllerBase
    {
        private readonly PlaseazaComandaHandler _plaseazaHandler;
        private readonly AnuleazaComandaHandler _anuleazaHandler;

        public ComenziController(
            PlaseazaComandaHandler plaseazaHandler,
            AnuleazaComandaHandler anuleazaHandler)
        {
            _plaseazaHandler = plaseazaHandler;
            _anuleazaHandler = anuleazaHandler;
        }

        /// <summary>
        /// Plasează o comandă nouă
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PlaseazaComanda([FromBody] PlaseazaComandaCommand command)
        {
            var result = await _plaseazaHandler.Handle(command);

            if (result.IsSuccess)
                return Ok(new { comandaId = result.Value, message = "Comanda a fost plasată cu succes!" });

            return BadRequest(new { error = result.Error });
        }

        /// <summary>
        /// Anulează o comandă existentă
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> AnuleazaComanda(Guid id, [FromBody] string motiv)
        {
            var command = new AnuleazaComandaCommand
            {
                ComandaId = id,
                Motiv = motiv ?? "Nu a fost specificat un motiv"
            };

            var result = await _anuleazaHandler.Handle(command);

            if (result.IsSuccess)
                return Ok(new { message = "Comanda a fost anulată cu succes!" });

            return BadRequest(new { error = result.Error });
        }
    }
}