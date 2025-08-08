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
            DataHoraInicio = r.DataHoraInicio,
            DataHoraFim = r.DataHoraFim,
            Status = r.Status,
            DataCancelamento = r.DataCancelamento

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
            DataHoraInicio = reserva.DataHoraInicio,
            DataHoraFim = reserva.DataHoraFim,
            Status = reserva.Status,
            DataCancelamento = reserva.DataCancelamento
        };
    }

    public async Task<ReservaDto> CriarAsync(ReservaCreateDto dto)
    {
        if (dto.DataHoraFim <= dto.DataHoraInicio)
            throw new ReservaDataInvalidaException();

        var conflitoReserva = await _unitOfWork.Reservas.ExisteConflitoReserva(dto.SalaId, dto.DataHoraInicio, dto.DataHoraFim);

        if (conflitoReserva)
            throw new ReservaConflitoHorarioException();

        var novaReserva = new Reserva
        {
            Id = Guid.NewGuid(),
            SalaId = dto.SalaId,
            UsuarioId = dto.UsuarioId,
            DataHoraInicio = dto.DataHoraInicio,
            DataHoraFim = dto.DataHoraFim,
            Status = Domain.Enum.ReservaStatus.Confirmada
        };

        await _unitOfWork.Reservas.AddAsync(novaReserva);
        await _unitOfWork.CommitAsync();

        return new ReservaDto
        {
            Id = novaReserva.Id,
            SalaId = novaReserva.SalaId,
            UsuarioId = novaReserva.Id,
            DataHoraInicio = novaReserva.DataHoraInicio,
            DataHoraFim = novaReserva.DataHoraFim,
            Status = novaReserva.Status
        };
    }

    public async Task CancelarAsync(Guid id)
    {
        var reserva = await _unitOfWork.Reservas.GetByIdAsync(id);

        if (reserva == null)
            throw new ReservaInexistenteException();

        var horasRestantes = (reserva.DataHoraInicio - DateTime.Now).TotalHours;

        if (horasRestantes < 24)
            throw new ReservaCancelamentoInvalidoException();

        reserva.Status = Domain.Enum.ReservaStatus.Cancelada;
        reserva.DataCancelamento = DateTime.Now;

        _unitOfWork.Reservas.Update(reserva);
        await _unitOfWork.CommitAsync();
    }
}
