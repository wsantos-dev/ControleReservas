using System;
using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
using ControleReservas.Domain;
using ControleReservas.Domain.Exceptions;
using ControleReservas.Domain.Interfaces;

namespace ControleReservas.Application.Services;

public class SalaService : ISalaService
{
    private readonly IUnitOfWork _unitOfWork;

    public SalaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<IEnumerable<SalaDto>> ObterSalasAsync()
    {
        var salas = await _unitOfWork.Salas.GetAllAsync();

        return salas.Select(s => new SalaDto
        {
            Id = s.Id,
            Nome = s.Nome!,
            Capacidade = s.Capacidade
        });
    }

    public async Task<SalaDto?> ObterPorIdAsync(Guid id)
    {
        var sala = await _unitOfWork.Salas.GetByIdAsync(id);
        if (sala == null) return null;

        return new SalaDto
        {
            Id = sala.Id,
            Nome = sala.Nome!,
            Capacidade = sala.Capacidade
        };
    }

    public async Task<SalaDto> CriarAsync(SalaDto dto)
    {
        var existe = await _unitOfWork.Salas.SalaExisteAsync(dto.Nome);

        if (existe)
            throw new SalaNomeDuplicadoException();

        var novaSala = new Sala
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Capacidade = dto.Capacidade
        };

        await _unitOfWork.Salas.AddAsync(novaSala);
        await _unitOfWork.CommitAsync();

        return new SalaDto { Id = novaSala.Id, Nome = novaSala.Nome, Capacidade = novaSala.Capacidade };
    }

    public async Task AtualizarAsync(SalaDto dto)
    {
        var salaExistente = await _unitOfWork.Salas.GetByIdAsync(dto.Id);

        if (salaExistente == null)
            throw new SalaNaoEncontradaException();

        salaExistente.Nome = dto.Nome;
        salaExistente.Capacidade = dto.Capacidade;

        _unitOfWork.Salas.Update(salaExistente);

        await _unitOfWork.CommitAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var sala = await _unitOfWork.Salas.GetByIdAsync(id);

        if (sala == null)
            throw new SalaNaoEncontradaException();

        _unitOfWork.Salas.Remove(sala);
        
        await _unitOfWork.CommitAsync();
    }

}
