using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
using ControleReservas.Domain;
using ControleReservas.Domain.Exceptions;
using ControleReservas.Domain.Interfaces;

namespace ControleReservas.Application.Services;

public class ReservaService : IReservaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    private readonly ISalaService _salaService;

    private readonly IUsuarioService _usuarioService;

    public ReservaService(IUnitOfWork unitOfWork,
        IEmailService emailService,
        ISalaService salaService,
        IUsuarioService usuarioService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _salaService = salaService;
        _usuarioService = usuarioService;
    }

    public async Task<IEnumerable<ReservaDto>> ObterReservasAsync()
    {
        var reservas = await _unitOfWork.Reservas.GetAllAsync();
        var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
        var salas = await _unitOfWork.Salas.GetAllAsync();

        reservas.Where(r => r.Usuario.Id == r.UsuarioId)
            .ToList()
            .ForEach(r =>
            {
                r.Usuario = usuarios.FirstOrDefault(u => u.Id == r.UsuarioId)!;
                r.Sala = salas.FirstOrDefault(s => s.Id == r.SalaId)!;
            });


        return reservas.Select(r => new ReservaDto
        {
            Id = r.Id,
            SalaId = r.SalaId,
            UsuarioId = r.UsuarioId,
            DataHoraInicio = r.DataHoraInicio,
            DataHoraFim = r.DataHoraFim,
            Status = r.Status,
            DataCancelamento = r.DataCancelamento,
            UsuarioNome = r.Usuario.Nome!,
            SalaNome = r.Sala.Nome!

        });
    }

    public async Task<ReservaDto?> ObterPorIdAsync(Guid id)
    {
        var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
        var salas = await _unitOfWork.Salas.GetAllAsync();

        var reserva = await _unitOfWork.Reservas.GetByIdAsync(id);
        
        if (reserva == null) return null;

        reserva.Sala = salas.FirstOrDefault(s => s.Id == reserva.SalaId)!;
        reserva.Usuario = usuarios.FirstOrDefault(u => u.Id == reserva.UsuarioId)!;


        return new ReservaDto
        {
            Id = reserva.Id,
            SalaId = reserva.SalaId,
            UsuarioId = reserva.UsuarioId,
            DataHoraInicio = reserva.DataHoraInicio,
            DataHoraFim = reserva.DataHoraFim,
            Status = reserva.Status,
            DataCancelamento = reserva.DataCancelamento,
            UsuarioNome = reserva.Usuario.Nome!,
            SalaNome = reserva.Sala.Nome!
        };
    }

    public async Task<ReservaDto> CriarAsync(ReservaCreateDto dto)
    {
        var horario = dto.DataHoraFim <= dto.DataHoraInicio;
        var conflitoReserva = await _unitOfWork.Reservas.ExisteConflitoReserva(dto.SalaId, dto.DataHoraInicio, dto.DataHoraFim);
        
        if (horario)
            throw new ReservaDataInvalidaException();

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

        var usuario = await _usuarioService.ObterPorIdAsync(novaReserva.UsuarioId)
        ?? throw new UsuarioNaoEncontradoException();

        var sala = await _salaService.ObterPorIdAsync(novaReserva.SalaId)
            ?? throw new SalaNaoEncontradaException();

        // Obter reservas existentes para a sala no mesmo intervalo
        var reservasExistentes = await _unitOfWork.Reservas
            .GetReservasPorSalaEPeriodoAsync(dto.SalaId, dto.DataHoraInicio, dto.DataHoraFim);

        if (reservasExistentes.Count() >= sala.Capacidade)
            throw new CapacidadeDaSalaExcedidaException($"Capacidade da sala excedida para o período solicitado. A sala só tem capacidade para {sala.Capacidade} pessoas.");


        await _unitOfWork.Reservas.AddAsync(novaReserva);
        await _unitOfWork.CommitAsync();

        await _emailService.EnviarEmailAsync(
            usuario.Email,
            $"Confirmação de Reserva - Sala {sala.Nome}",
            $@"<p>Prezado usuário {usuario.Nome}</p>
               <p>Sua reserva para a sala {sala.Nome} foi <b>confirmada!</b></p>
               <p>Horário de Início: {novaReserva.DataHoraInicio:dd/MM/yyyy HH:mm}</p> 
               <p>Horário de Término: {novaReserva.DataHoraFim:dd/MM/yyyy HH:mm}</p>
            "
        );

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

        if (horasRestantes <= 0)
            throw new CancelamentoExpiradoException();

        if (horasRestantes < 24)
            throw new ReservaCancelamentoInvalidoException();

        reserva.Status = Domain.Enum.ReservaStatus.Cancelada;
        reserva.DataCancelamento = DateTime.Now;


        var usuario = _usuarioService.ObterPorIdAsync(reserva.UsuarioId).GetAwaiter().GetResult()!;
        var sala = _salaService.ObterPorIdAsync(reserva.SalaId).GetAwaiter().GetResult()!;

        _unitOfWork.Reservas.Update(reserva);
        await _unitOfWork.CommitAsync();
        
         await _emailService.EnviarEmailAsync(
            usuario.Email,
            $"Confirmação de Cancelamento de Reserva - Sala {sala.Nome}",
            $@"<p>Prezado usuário {usuario.Nome}</p>
               <p>Sua reserva para a sala {sala.Nome} foi <b>cancelada!</b></p>
               <p>Detalhes do cancelamento:</p>
               <p>Horário agendado anteriormente: Data de Inicio: {reserva.DataHoraInicio::dd/MM/yyyy HH:mm} | Data de Términio: {reserva.DataHoraFim:dd/MM/yyyy HH:mm}</p>
               <p>Data do Cancelamento: {reserva.DataCancelamento:dd/MM/yyyy HH:mm}</p>
            "
        );
    }
}
