using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
using ControleReservas.Domain;
using ControleReservas.Domain.Exceptions;
using ControleReservas.Domain.Interfaces;

namespace ControleReservas.Application.Services;

public class ReservaService : IReservaService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ReservaDto>> ObterReservasAsync()
    {
        var reservas = await _unitOfWork.Reservas.GetAllAsync();

        return reservas.Select(r => new ReservaDto
        {
            Id = r.Id,
            SalaId = r.SalaId,
            UsuarioId = r.UsuarioId,
            DataHoraReserva = r.DataHoraReserva,
            Status = r.Status

        });
    }

    public async Task<ReservaDto?> ObterPorIdAsync(Guid id)
    {
        var reserva = await _unitOfWork.Reservas.GetByIdAsync(id);
        if (reserva == null) return null;

        return new ReservaDto
        {
            Id = reserva.Id,
            SalaId = reserva.SalaId,
            UsuarioId = reserva.UsuarioId,
            DataHoraReserva = reserva.DataHoraReserva,
            Status = reserva.Status

        };
    }

    public async Task CriarAsync(ReservaCreateDto dto)
    {
        var conflitoReserva = await _unitOfWork.Reservas.ExisteConflitoReserva(dto.SalaId, dto.DateHoraReserva);

        if (conflitoReserva)
            throw new ReservaConflitoHorarioException();

        var novaReserva = new Reserva
        {
            Id = Guid.NewGuid(),
            SalaId = dto.SalaId,
            UsuarioId = dto.UsuarioId,
            DataHoraReserva = dto.DateHoraReserva,
            Status = Domain.Enum.ReservaStatus.Confirmada
        };

        await _unitOfWork.Reservas.AddAsync(novaReserva);
        await _unitOfWork.CommitAsync();
    }

    public async Task CancelarAsync(Guid id)
    {
        var reserva = await _unitOfWork.Reservas.GetByIdAsync(id);

        if (reserva == null)
            throw new ReservaInexistenteException();

        var horasRestantes = (reserva.DataHoraReserva - DateTime.Now).TotalHours;

        if (horasRestantes < 24)
            throw new ReservaCancelamentoInvalidoException();

        reserva.Status = Domain.Enum.ReservaStatus.Cancelada;

        _unitOfWork.Reservas.Update(reserva);
        await _unitOfWork.CommitAsync();
    }

   

   

    
}
