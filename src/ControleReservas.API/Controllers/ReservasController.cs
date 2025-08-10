using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
using ControleReservas.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleReservas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _reservaService.ObterReservasAsync());


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var reserva = await _reservaService.ObterPorIdAsync(id);

            if (reserva is null) return NotFound();

            return Ok(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReservaCreateDto dto)
        {
            var reserva = await _reservaService.CriarAsync(dto);

            return CreatedAtAction(nameof(GetById), new { reserva.Id }, reserva);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            try
            {
                await _reservaService.CancelarAsync(id);
                return NoContent();

            }
            catch (CancelamentoExpiradoException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (ReservaCancelamentoInvalidoException ex)
            {

                return BadRequest(new { mensagem = ex.Message });
            }


        }
    }
}
