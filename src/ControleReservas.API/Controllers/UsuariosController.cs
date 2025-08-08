using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
using ControleReservas.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ControleReservas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _usuarioService.ObterUsuariosAsync());


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);

        if (usuario == null) return NotFound();

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UsuarioDto dto)
    {
       var usuarioCriado = await _usuarioService.CriarAsync(dto);

        return CreatedAtAction(nameof(GetById), new { usuarioCriado.Id }, usuarioCriado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UsuarioDto dto)
    {

        if (id != dto.Id)
        {
            return BadRequest();
        }

        await _usuarioService.AtualizarAsync(dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        await _usuarioService.RemoverAsync(usuario.Id);

        return NoContent();
    }
}
