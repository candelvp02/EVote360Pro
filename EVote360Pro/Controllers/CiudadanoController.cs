using EVote360Pro.Core.Application.Dtos.Ciudadano;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.Ciudadano;
using EVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class CiudadanoController : Controller
    {
        private readonly ICiudadanoService _ciudadanoService;
        private readonly IUserSession _userSession;

        public CiudadanoController(ICiudadanoService ciudadanoService, IUserSession userSession)
        {
            _ciudadanoService = ciudadanoService;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var listCiudadanos = await _ciudadanoService.GetAll();
            var listVm = listCiudadanos.Select(s => new CiudadanoViewModel()
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Apellido = s.Apellido,
                CorreoElectronico = s.CorreoElectronico,
                NumeroDocumento = s.NumeroDocumento,
                Activo = s.Activo
            }).ToList();

            return View(listVm);
        }

        public IActionResult Create()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            return View(new SaveCiudadanoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCiudadanoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid) return View(vm);

            SaveCiudadanoDto dto = new()
            {
                Id = 0,
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                CorreoElectronico = vm.CorreoElectronico,
                NumeroDocumento = vm.NumeroDocumento,
                Activo = true
            };

            await _ciudadanoService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var entity = await _ciudadanoService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            SaveCiudadanoViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                CorreoElectronico = entity.CorreoElectronico,
                NumeroDocumento = entity.NumeroDocumento,
                Activo = entity.Activo
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCiudadanoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid) return View(vm);

            SaveCiudadanoDto dto = new()
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                CorreoElectronico = vm.CorreoElectronico,
                NumeroDocumento = vm.NumeroDocumento,
                Activo = vm.Activo
            };

            await _ciudadanoService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var entity = await _ciudadanoService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            DeleteCiudadanoViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                NumeroDocumento = entity.NumeroDocumento
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCiudadanoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            await _ciudadanoService.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            await _ciudadanoService.CambiarEstadoAsync(id);
            return RedirectToAction("Index");
        }
    }
}