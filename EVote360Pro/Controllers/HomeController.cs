using EVote360Pro.Core.Application.Dtos.Usuario;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserSession _userSession;
        private readonly IEleccionService _eleccionService;

        public HomeController(IUserSession userSession, IEleccionService eleccionService)
        {
            _userSession = userSession;
            _eleccionService = eleccionService;
        }

        public async Task<IActionResult> Index()
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var anos = await _eleccionService.GetAnosConElecciones();
            int anoSeleccionado = anos.FirstOrDefault();

            var elecciones = anoSeleccionado > 0
                ? await _eleccionService.GetByAno(anoSeleccionado)
                : new List<Core.Application.Dtos.Eleccion.EleccionDto>();

            ViewBag.Anos = anos;
            ViewBag.AnoSeleccionado = anoSeleccionado;
            ViewBag.Elecciones = elecciones;

            return View();
        }

        public async Task<IActionResult> ResumenPorAno(int ano)
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var elecciones = await _eleccionService.GetByAno(ano);
            var anos = await _eleccionService.GetAnosConElecciones();

            ViewBag.Anos = anos;
            ViewBag.AnoSeleccionado = ano;
            ViewBag.Elecciones = elecciones;

            return View("Index");
        }
    }
}