using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleReservas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalasController : ControllerBase
{
    private readonly ISalaService _salaService;

    public SalasController(ISalaService salaService)
    {
        _salaService = salaService;
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
        await _salaService.CriarAsync(dto);
       
        return CreatedAtAction(nameof(GetById), new { dto.Id }, dto);
    }
}
