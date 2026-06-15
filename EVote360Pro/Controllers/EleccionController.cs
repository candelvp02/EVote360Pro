using EVote360Pro.Core.Application.Dtos.Eleccion;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.Eleccion;
using EVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class EleccionController : Controller
    {
        private readonly IEleccionService _eleccionService;
        private readonly IUserSession _userSession;

        public EleccionController(IEleccionService eleccionService, IUserSession userSession)
        {
            _eleccionService = eleccionService;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var listElecciones = await _eleccionService.GetAll();
            var listVm = listElecciones.Select(s => new EleccionViewModel()
            {
                Id = s.Id,
                Nombre = s.Nombre,
                FechaRealizacion = s.FechaRealizacion,
                Estado = s.Estado,
                Activo = s.Activo
            }).ToList();

            return View(listVm);
        }

        public IActionResult Create()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            return View(new SaveEleccionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveEleccionViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid) return View(vm);

            SaveEleccionDto dto = new()
            {
                Id = 0,
                Nombre = vm.Nombre,
                FechaRealizacion = vm.FechaRealizacion,
                Activo = true
            };

            await _eleccionService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var entity = await _eleccionService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            if (entity.Estado != EstadoEleccion.Pendiente)
            {
                TempData["Error"] = "Solo se pueden eliminar elecciones en estado Pendiente.";
                return RedirectToAction("Index");
            }

            EleccionViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                FechaRealizacion = entity.FechaRealizacion,
                Estado = entity.Estado
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EleccionViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            await _eleccionService.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Activar(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            bool hayActiva = await _eleccionService.GetEleccionActiva() != null;
            if (hayActiva)
            {
                TempData["Error"] = "Ya existe una elección activa. Solo puede haber una elección activa a la vez.";
                return RedirectToAction("Index");
            }

            bool resultado = await _eleccionService.ActivarEleccionAsync(id);
            if (!resultado)
            {
                TempData["Error"] = "No se pudo activar la elección. Verifique que haya puestos activos configurados.";
            }
            else
            {
                TempData["Exito"] = "Elección activada exitosamente.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Finalizar(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            bool resultado = await _eleccionService.FinalizarEleccionAsync(id);
            if (!resultado)
            {
                TempData["Error"] = "No se pudo finalizar la elección.";
            }
            else
            {
                TempData["Exito"] = "Elección finalizada exitosamente.";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Resultados(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var resultados = await _eleccionService.GetResultados(id);
            if (resultados == null)
            {
                TempData["Error"] = "No se encontraron resultados para esta elección.";
                return RedirectToAction("Index");
            }

            return View(resultados);
        }
    }
}