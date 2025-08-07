using System;
using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
using ControleReservas.Domain;
using ControleReservas.Domain.Exceptions;
using ControleReservas.Domain.Interfaces;

namespace ControleReservas.Application.Services;

public class UsuarioService : IUsuarioService
{

    private readonly IUnitOfWork _unitOfWork;

    public UsuarioService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UsuarioDto>> ObterUsuariosAsync()
    {
        var usuarios = await _unitOfWork.Usuarios.GetAllAsync();

        return usuarios.Select(u => new UsuarioDto
        {
            Id = u.Id,
            Nome = u.Nome!,
            Email = u.Email!
        });
    }

    public async Task<UsuarioDto?> ObterPorIdAsync(Guid id)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);

        if (usuario == null) return null;

        return new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome!,
            Email = usuario.Email!
        };
    }

       public async Task CriarAsync(UsuarioDto dto)
    {
        var existe = await _unitOfWork.Usuarios.ObterPorEmailAsync(dto.Email);
        if (existe != null)
            throw new UsuarioEmailDuplicadoException();

        var novoUsuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Email = dto.Email
        };

        await _unitOfWork.Usuarios.AddAsync(novoUsuario);
        await _unitOfWork.CommitAsync();
    }
    
   

}
