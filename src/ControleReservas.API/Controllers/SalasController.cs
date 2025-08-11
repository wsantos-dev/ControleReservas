using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
using ControleReservas.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ControleReservas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalasController : ControllerBase
{
    private readonly ISalaService _salaService;
    private readonly IReservaService _reservaService;

    public SalasController(ISalaService salaService, IReservaService reservaService)
    {
        _salaService = salaService;
        _reservaService = reservaService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _salaService.ObterSalasAsync());


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var sala = await _salaService.ObterPorIdAsync(id);

        if (sala == null) return NotFound();

        return Ok(sala);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SalaDto dto)
    {
        var salaCriada = await _salaService.CriarAsync(dto);

        return CreatedAtAction(nameof(GetById), new { salaCriada.Id }, salaCriada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] SalaDto dto)
    {

        if (id != dto.Id)
        {
            return BadRequest();
        }

        await _salaService.AtualizarAsync(dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            var sala = await _salaService.ObterPorIdAsync(id);
            var reserva = await _reservaService.ObterReservasAsync();

            var existeAlgumaReserva = reserva.Any(r => r.SalaId == sala?.Id);
            var quantidadeReservas = reserva.Count();

            if (existeAlgumaReserva)
            {
                throw new SalaComReservaExistenteException(quantidadeReservas);
            }

            if (sala == null)
            {
                return NotFound();
            }

            await _salaService.RemoverAsync(sala.Id);

            return NoContent();
        }
        catch (SalaComReservaExistenteException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
