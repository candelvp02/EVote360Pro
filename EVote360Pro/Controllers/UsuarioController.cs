using EVote360Pro.Core.Application.Dtos.Usuario;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.Usuario;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUserSession _userSession;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioService usuarioService, IUserSession userSession)
        {
            _usuarioService = usuarioService;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var listUsuarios = await _usuarioService.GetAll();
            var listVm = listUsuarios.Select(s => new UsuarioViewModel()
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Apellido = s.Apellido,
                CorreoElectronico = s.CorreoElectronico,
                NombreUsuario = s.NombreUsuario,
                Rol = s.Rol,
                Activo = s.Activo
            }).ToList();

            return View(listVm);
        }

        public IActionResult Create()
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            return View(new SaveUsuarioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUsuarioViewModel vm)
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid) return View(vm);

            SaveUsuarioDto dto = new()
            {
                Id = 0,
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                CorreoElectronico = vm.CorreoElectronico,
                NombreUsuario = vm.NombreUsuario,
                Contrasena = vm.Contrasena,
                ConfirmacionContrasena = vm.ConfirmacionContrasena,
                Rol = vm.Rol,
                Activo = true
            };

            await _usuarioService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var entity = await _usuarioService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            SaveUsuarioViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                CorreoElectronico = entity.CorreoElectronico,
                NombreUsuario = entity.NombreUsuario,
                Rol = entity.Rol,
                Activo = entity.Activo
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUsuarioViewModel vm)
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid) return View(vm);

            SaveUsuarioDto dto = new()
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                CorreoElectronico = vm.CorreoElectronico,
                NombreUsuario = vm.NombreUsuario,
                Contrasena = vm.Contrasena,
                ConfirmacionContrasena = vm.ConfirmacionContrasena,
                Rol = vm.Rol,
                Activo = vm.Activo
            };

            await _usuarioService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var entity = await _usuarioService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            DeleteUsuarioViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                NombreUsuario = entity.NombreUsuario
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteUsuarioViewModel vm)
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            await _usuarioService.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            UsuarioDto? usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            await _usuarioService.CambiarEstadoAsync(id);
            return RedirectToAction("Index");
        }
    }
}