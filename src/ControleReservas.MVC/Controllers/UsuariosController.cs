using ControleReservas.MVC.Models;
using ControleReservas.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleReservas.MVC.Controllers
{
    public class UsuariosController : Controller
    {
        private IUsuarioApiService _usuarioApi;

        public UsuariosController(IUsuarioApiService usuarioApiService)
        {
            _usuarioApi = usuarioApiService;
        }

         public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioApi.ObterUsuariosAsync();

            return View(usuarios);
        }

        public IActionResult Criar()
        {

            return View(new UsuarioViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(UsuarioViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                await _usuarioApi.CriarAsync(vm);
                TempData["Success"] = "Usuário criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(vm);
            }
        }
        
   
        public async Task<IActionResult> Editar(Guid id)
        {
            var usuario = await _usuarioApi.ObterPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Guid id, UsuarioViewModel vm)
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
                await _usuarioApi.AtualizarAsync(vm);
                TempData["Success"] = "usuario atualizada com sucesso!";
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
            var usuario = await _usuarioApi.ObterPorIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }
    }
}
