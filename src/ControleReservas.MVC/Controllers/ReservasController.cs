using ControleReservas.MVC.Models;
using ControleReservas.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleReservas.MVC.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IReservaApiService _reservaApi;
        private readonly ISalaApiService _salaApi;
        private readonly IUsuarioApiService _usuarioApi;

        public ReservasController(
            IReservaApiService reservaApiService,
            ISalaApiService salaApiService,
            IUsuarioApiService usuarioApiService)
        {
            _reservaApi = reservaApiService;
            _salaApi = salaApiService;
            _usuarioApi = usuarioApiService;
        }

        public async Task<IActionResult> Index()
        {
            var reservas = await _reservaApi.ObterReservasAsync();

            return View(reservas);
        }

        public async Task<IActionResult> Criar()
        {
            var dataHoraMinima = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.DataMinima = dataHoraMinima;


            var salas = await _salaApi.ObterSalasAsync();
            ViewBag.Salas = salas;

            var usuarios = await _usuarioApi.ObterUsuariosAsync();
            ViewBag.Usuarios = usuarios;

            return View(new ReservaCreateViewModel());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(ReservaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Salas = await _salaApi.ObterSalasAsync();
                ViewBag.Usuarios = await _usuarioApi.ObterUsuariosAsync();

                return View(vm);
            }

            try
            {
                await _reservaApi.CriarAsync(vm);
                TempData["Success"] = "Reserva criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Salas = await _salaApi.ObterSalasAsync();
                ViewBag.Usuarios = await _usuarioApi.ObterUsuariosAsync();
                return View(vm);
            }
        }

        // GET: Reservas/Editar/{id}
        public async Task<IActionResult> Editar(Guid id)
        {
            var reserva = await _reservaApi.ObterPorIdAsync(id);
            if (reserva == null)
                return NotFound();

            var salas = await _salaApi.ObterSalasAsync();
            ViewBag.Salas = salas;

            var usuarios = await _usuarioApi.ObterUsuariosAsync();
            ViewBag.Usuarios = usuarios;

            return View(reserva);
        }

        // POST: Reservas/Editar/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, ReservaViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Salas = await _salaApi.ObterSalasAsync();
                ViewBag.Usuarios = await _usuarioApi.ObterUsuariosAsync();
                return View(vm);
            }

            try
            {
                await _reservaApi.EditarAsync(id, vm);
                TempData["Success"] = "Reserva editada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                TempData["Error"] = $"Erro ao tentar editar uma reserva: {ex.Message}";
                ViewBag.Salas = await _salaApi.ObterSalasAsync();
                ViewBag.Usuarios = await _usuarioApi.ObterUsuariosAsync();
                return RedirectToAction(nameof(Index));
            }
        }



        public async Task<IActionResult> Cancelar(Guid id)
        {
            var reserva = await _reservaApi.ObterPorIdAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }
            return View(reserva);
        }

    
        [HttpPost, ActionName("Cancelar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarConfirmacao(Guid id)
        {
            try
            {
                await _reservaApi.CancelarAsync(id);
                TempData["Success"] = "Reserva cancelada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao cancelar a reserva: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
