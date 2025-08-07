using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
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
        await _usuarioService.CriarAsync(dto);
       
        return CreatedAtAction(nameof(GetById), new { dto.Id }, dto);
    }
}
