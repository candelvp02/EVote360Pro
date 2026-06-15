using EVote360Pro.Core.Application.Dtos.PartidoPolitico;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.PartidoPolitico;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class PartidoPoliticoController : Controller
    {
        private readonly IPartidoPoliticoService _partidoPoliticoService;
        private readonly IEleccionService _eleccionService;
        private readonly IUserSession _userSession;

        public PartidoPoliticoController(IPartidoPoliticoService partidoPoliticoService, IEleccionService eleccionService, IUserSession userSession)
        {
            _partidoPoliticoService = partidoPoliticoService;
            _eleccionService = eleccionService;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var listPartidos = await _partidoPoliticoService.GetAll();
            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            ViewBag.HayEleccionActiva = eleccionActiva != null;

            var listVm = listPartidos.Select(s => new PartidoPoliticoViewModel()
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Descripcion = s.Descripcion,
                Siglas = s.Siglas,
                LogoUrl = s.LogoUrl,
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
                TempData["Error"] = "No se pueden crear partidos mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            return View(new SavePartidoPoliticoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePartidoPoliticoViewModel vm, IFormFile? logoFile)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid) return View(vm);

            SavePartidoPoliticoDto dto = new()
            {
                Id = 0,
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                Siglas = vm.Siglas,
                LogoUrl = string.Empty,
                Activo = true
            };

            var returnDto = await _partidoPoliticoService.AddAsync(dto);

            if (returnDto != null && logoFile != null)
            {
                string logoUrl = FileManager.Upload(logoFile, returnDto.Id, "Partidos");
                dto.Id = returnDto.Id;
                dto.LogoUrl = logoUrl;
                await _partidoPoliticoService.UpdateAsync(dto);
            }

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
                TempData["Error"] = "No se pueden editar partidos mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            var entity = await _partidoPoliticoService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            SavePartidoPoliticoViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion,
                Siglas = entity.Siglas,
                LogoUrl = entity.LogoUrl,
                Activo = entity.Activo
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePartidoPoliticoViewModel vm, IFormFile? logoFile)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid) return View(vm);

            string logoUrl = vm.LogoUrl;
            if (logoFile != null)
            {
                logoUrl = FileManager.Upload(logoFile, vm.Id, "Partidos");
            }

            SavePartidoPoliticoDto dto = new()
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                Siglas = vm.Siglas,
                LogoUrl = logoUrl,
                Activo = vm.Activo
            };

            await _partidoPoliticoService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var entity = await _partidoPoliticoService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            DeletePartidoPoliticoViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Siglas = entity.Siglas
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeletePartidoPoliticoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            await _partidoPoliticoService.DeleteAsync(vm.Id);
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

            await _partidoPoliticoService.CambiarEstadoAsync(id);
            return RedirectToAction("Index");
        }
    }
}