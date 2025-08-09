using ControleReservas.MVC.Models;
using ControleReservas.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleReservas.MVC.Controllers
{
    public class SalaController : Controller
    {
        private readonly ISalaApiService _salaApi;

        public SalaController(ISalaApiService salaApiService)
        {
            _salaApi = salaApiService;
        }

        public async Task<IActionResult> Index()
        {
            var salas = await _salaApi.ObterSalasAsync();

            return View(salas);
        }

        public IActionResult Criar()
        {

            return View(new SalaViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(SalaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                await _salaApi.CriarAsync(vm);
                TempData["Success"] = "Sala criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(vm);
            }
        }
        
        // Método GET para exibir o formulário de edição
        public async Task<IActionResult> Editar(Guid id)
        {
            var sala = await _salaApi.ObterPorIdAsync(id);
            if (sala == null)
            {
                return NotFound();
            }
            return View(sala);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, SalaViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                await _salaApi.AtualizarAsync(vm);
                TempData["Success"] = "Sala atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> Remover(Guid id)
        {
            var sala = await _salaApi.ObterPorIdAsync(id);

            if (sala == null)
            {
                return NotFound();
            }
            return View(sala);
        }


    }
}
