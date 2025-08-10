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

        public ReservaServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _emailServiceMock = new Mock<IEmailService>();
            _salaServiceMock = new Mock<ISalaService>();
            _usuarioServiceMock = new Mock<IUsuarioService>();

            _reservaService = new ReservaService(
                _unitOfWorkMock.Object,
                _emailServiceMock.Object,
                _salaServiceMock.Object,
                _usuarioServiceMock.Object
            );
        }

        #region CriarAsync Tests

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
                SalaId = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
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
                SalaId = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                DataHoraInicio = DateTime.Now.AddHours(1),
                DataHoraFim = DateTime.Now.AddHours(2)
            };

            _unitOfWorkMock.Setup(u => u.Reservas.ExisteConflitoReserva(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                           .ReturnsAsync(false);

            _salaServiceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync(new SalaDto { Nome = "Sala Teste" });


            _usuarioServiceMock.Setup(u => u.ObterPorIdAsync(dto.UsuarioId))
                               .ReturnsAsync(new UsuarioDto { Id = dto.UsuarioId, Nome = "Usuário 1", Email = "teste@teste.com" });

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
        public async Task CancelarAsync_DeveCancelarReserva_SalvarEEnviarEmail_SeValido()
        {
          
            var sala = new SalaDto
            {
                Id = new Guid("b47063e6-7b5d-4095-965b-7fb5b6f4cfe3"),
                Nome = "Sala de Reunião Beta"
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