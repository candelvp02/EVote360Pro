using EVote360Pro.Core.Application.Dtos.PuestoElectivo;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.PuestoElectivo;
using EVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class PuestoElectivoController : Controller
    {
        private readonly IPuestoElectivoService _puestoElectivoService;
        private readonly IEleccionService _eleccionService;
        private readonly IUserSession _userSession;

        public PuestoElectivoController(IPuestoElectivoService puestoElectivoService, IEleccionService eleccionService, IUserSession userSession)
        {
            _puestoElectivoService = puestoElectivoService;
            _eleccionService = eleccionService;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var listPuestos = await _puestoElectivoService.GetAll();
            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            ViewBag.HayEleccionActiva = eleccionActiva != null;

            var listVm = listPuestos.Select(s => new PuestoElectivoViewModel()
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Descripcion = s.Descripcion,
                Activo = s.Activo
            }).ToList();

            return View(listVm);
        }

        public async Task<IActionResult> Create()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se pueden crear puestos electivos mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            return View(new SavePuestoElectivoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePuestoElectivoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid) return View(vm);

            SavePuestoElectivoDto dto = new()
            {
                Id = 0,
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                Activo = true
            };

            await _puestoElectivoService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se pueden editar puestos electivos mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            var entity = await _puestoElectivoService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            SavePuestoElectivoViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion,
                Activo = entity.Activo
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePuestoElectivoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid) return View(vm);

            SavePuestoElectivoDto dto = new()
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                Activo = vm.Activo
            };

            await _puestoElectivoService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var entity = await _puestoElectivoService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            DeletePuestoElectivoViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeletePuestoElectivoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            await _puestoElectivoService.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se puede cambiar el estado mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            await _puestoElectivoService.CambiarEstadoAsync(id);
            return RedirectToAction("Index");
        }
    }
}