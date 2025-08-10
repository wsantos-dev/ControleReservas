using ControleReservas.Application.DTOs;
using ControleReservas.Application.Interfaces;
using ControleReservas.Application.Services;
using ControleReservas.Domain;
using ControleReservas.Domain.Enum;
using ControleReservas.Domain.Exceptions;
using ControleReservas.Domain.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ControleReservas.Tests
{
    public class ReservaServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ISalaService> _salaServiceMock;
        private readonly Mock<IUsuarioService> _usuarioServiceMock;
        private readonly ReservaService _reservaService;

        private readonly Mock<IReservaRepository> _reservaRepositoryMock;
        private readonly Mock<ISalaRepository> _salaRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;


        public ReservaServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _emailServiceMock = new Mock<IEmailService>();
            _salaServiceMock = new Mock<ISalaService>();
            _usuarioServiceMock = new Mock<IUsuarioService>();

            _reservaRepositoryMock = new Mock<IReservaRepository>();
            _salaRepositoryMock = new Mock<ISalaRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();

            _unitOfWorkMock.Setup(u => u.Reservas).Returns(_reservaRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Salas).Returns(_salaRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Usuarios).Returns(_usuarioRepositoryMock.Object);


            _reservaService = new ReservaService(
                _unitOfWorkMock.Object,
                _emailServiceMock.Object,
                _salaServiceMock.Object,
                _usuarioServiceMock.Object
            );
        }

        #region CriarAsync Tests

        [Fact]
        public async Task CriarAsync_DeveCriarReserva_QuandoCapacidadeNaoFoiAtingida()
        {
            // Arrange
            var dto = new ReservaCreateDto
            {
                SalaId = new Guid("b47063e6-7b5d-4095-965b-7fb5b6f4cfe3"), // Sala de Reunião Beta
                UsuarioId = new Guid("9ca38335-cdb6-4a78-8031-f28136e67b11"), // Wellington Santos
                DataHoraInicio = DateTime.Now.AddDays(60).AddHours(10),
                DataHoraFim = DateTime.Now.AddDays(60).AddHours(12)
            };

            var sala = new Sala { Id = dto.SalaId, Nome = "Sala de Comunicação Digital", Capacidade = 2 };
            var usuario = new Usuario { Id = dto.UsuarioId, Nome = "Wellington Santos", Email = "wellington.bezerra.santos@outlook.com" };
            var reservasExistentes = new List<Reserva>
            {
                new Reserva { Id = Guid.NewGuid(), SalaId = dto.SalaId }
            };

            _salaServiceMock.Setup(s => s.ObterPorIdAsync(dto.SalaId))
                .ReturnsAsync(new SalaDto { Id = sala.Id, Nome = sala.Nome, Capacidade = sala.Capacidade });

            _usuarioServiceMock.Setup(u => u.ObterPorIdAsync(dto.UsuarioId))
                .ReturnsAsync(new UsuarioDto { Id = usuario.Id, Nome = usuario.Nome, Email = usuario.Email });

       
            _unitOfWorkMock.Setup(u => u.Reservas.ExisteConflitoReserva(dto.SalaId, dto.DataHoraInicio, dto.DataHoraFim))
                           .ReturnsAsync(false);

            _reservaRepositoryMock.Setup(r => r.GetReservasPorSalaEPeriodoAsync(dto.SalaId, dto.DataHoraInicio, dto.DataHoraFim))
                .ReturnsAsync(reservasExistentes);

            _reservaRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Reserva>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.Reservas).Returns(_reservaRepositoryMock.Object);

            var resultado = await _reservaService.CriarAsync(dto);

            Assert.NotNull(resultado);
            Assert.Equal(dto.SalaId, resultado.SalaId);
            _reservaRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Reserva>()), Times.Once);
        }

        [Fact]
        public async Task CriarAsync_DeveLancarReservaDataInvalidaException_SeDataFimMenorOuIgualInicio()
        {
            var dto = new ReservaCreateDto
            {
                SalaId = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                DataHoraInicio = DateTime.Now.AddHours(2),
                DataHoraFim = DateTime.Now.AddHours(1)
            };

            await Assert.ThrowsAsync<ReservaDataInvalidaException>(() => _reservaService.CriarAsync(dto));
        }

        [Fact]
        public async Task CriarAsync_DeveLancarReservaConflitoHorarioException_SeExisteConflito()
        {
            var dto = new ReservaCreateDto
            {
                SalaId = new Guid("c9798a11-bfe5-49fd-bff2-28b6ab868609"), // 
                UsuarioId = new Guid("9ca38335-cdb6-4a78-8031-f28136e67b11"), // Wellington Santos
                DataHoraInicio = DateTime.Now.AddHours(1),
                DataHoraFim = DateTime.Now.AddHours(2)
            };

            _unitOfWorkMock.Setup(u => u.Reservas.ExisteConflitoReserva(dto.SalaId, dto.DataHoraInicio, dto.DataHoraFim))
                           .ReturnsAsync(true);

            await Assert.ThrowsAsync<ReservaConflitoHorarioException>(() => _reservaService.CriarAsync(dto));
        }

        [Fact]
        public async Task CriarAsync_DeveSalvarEEnviarEmail_SeTudoValido()
        {
            var dto = new ReservaCreateDto
            {
                SalaId = new Guid("b47063e6-7b5d-4095-965b-7fb5b6f4cfe3"),
                UsuarioId = new Guid("9CA38335-CDB6-4A78-8031-F28136E67B11"),
                DataHoraInicio = DateTime.Now.AddHours(1),
                DataHoraFim = DateTime.Now.AddHours(2)
            };

            _unitOfWorkMock.Setup(u => u.Reservas.ExisteConflitoReserva(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                           .ReturnsAsync(false);

            _salaServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync(new SalaDto { Id = dto.SalaId, Nome = "Sala de Reunião Beta", Capacidade = 5 });


            _usuarioServiceMock.Setup(u => u.ObterPorIdAsync(dto.UsuarioId))
                               .ReturnsAsync(new UsuarioDto { Id = dto.UsuarioId, Nome = "Wellington Santos", Email = "wellington.bezerra.santos@outlook.com" });

            await _reservaService.CriarAsync(dto);

            _unitOfWorkMock.Verify(u => u.Reservas.AddAsync(It.IsAny<Reserva>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
            _emailServiceMock.Verify(e => e.EnviarEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        #endregion

        #region CancelarAsync Tests

        [Fact]
        public async Task CancelarAsync_DeveLancarReservaInexistenteException_SeNaoEncontrar()
        {
            _unitOfWorkMock.Setup(u => u.Reservas.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync((Reserva)null!);

            await Assert.ThrowsAsync<ReservaInexistenteException>(() => _reservaService.CancelarAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task CancelarAsync_DeveLancarCancelamentoExpiradoException_SeReservaJaIniciou()
        {
            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                DataHoraInicio = DateTime.Now.AddMinutes(-10)
            };

            _unitOfWorkMock.Setup(u => u.Reservas.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(reserva);

            await Assert.ThrowsAsync<CancelamentoExpiradoException>(() => _reservaService.CancelarAsync(reserva.Id));
        }

        [Fact]
        public async Task CancelarAsync_DeveLancarReservaCancelamentoInvalidoException_SeFaltarMenosDe24Horas()
        {
            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                DataHoraInicio = DateTime.Now.AddHours(10)
            };

            _unitOfWorkMock.Setup(u => u.Reservas.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(reserva);

            await Assert.ThrowsAsync<ReservaCancelamentoInvalidoException>(() => _reservaService.CancelarAsync(reserva.Id));
        }

        [Fact]
        public async Task CancelarAsync_DeveCancelarReserva_EnviandoEmail_SeValido()
        {
          
            var sala = new SalaDto
            {
                Id = new Guid("b47063e6-7b5d-4095-965b-7fb5b6f4cfe3"),
                Nome = "Sala de Reunião Beta",
                Capacidade = 5
            };

            var usuario = new UsuarioDto
            {
                Id = new Guid("9ca38335-cdb6-4a78-8031-f28136e67b11"),
                Nome = "Wellington Santos",
                Email = "wellington.bezerra.santos@outlook.com"
            };

            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                SalaId = sala.Id,
                UsuarioId = usuario.Id,
                DataHoraInicio = DateTime.UtcNow.AddHours(40),
                DataHoraFim = DateTime.UtcNow.AddHours(45),
                Status = ReservaStatus.Confirmada 
            };

      
            _unitOfWorkMock
                .Setup(u => u.Reservas.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(reserva);

          
            _usuarioServiceMock
                .Setup(u => u.ObterPorIdAsync(usuario.Id))
                .ReturnsAsync(usuario);

            _salaServiceMock
                .Setup(s => s.ObterPorIdAsync(sala.Id))
                .ReturnsAsync(sala);

          
            _unitOfWorkMock
                .Setup(u => u.Reservas.Update(It.IsAny<Reserva>()));

            _unitOfWorkMock
                .Setup(u => u.CommitAsync())
                .Returns(Task.CompletedTask);

           
            _emailServiceMock
                .Setup(e => e.EnviarEmailAsync(usuario.Email, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);


            await _reservaService.CancelarAsync(reserva.Id);

          
            Assert.Equal(ReservaStatus.Cancelada, reserva.Status);

            _unitOfWorkMock.Verify(u => u.Reservas.Update(It.Is<Reserva>(
                r => r.Id == reserva.Id && r.Status == ReservaStatus.Cancelada
            )), Times.Once);

            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);

            _emailServiceMock.Verify(e => e.EnviarEmailAsync(
                It.Is<string>(to => to == usuario.Email),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Once);

        }

    }
}
#endregion CancelarAsync Tests