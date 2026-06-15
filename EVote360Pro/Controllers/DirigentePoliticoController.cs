using EVote360Pro.Core.Application.Dtos.DirigentePolitico;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.DirigentePolitico;
using EVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class DirigentePoliticoController : Controller
    {
        private readonly IDirigentePoliticoService _dirigenteService;
        private readonly IUserSession _userSession;

        public DirigentePoliticoController(IDirigentePoliticoService dirigenteService, IUserSession userSession)
        {
            _dirigenteService = dirigenteService;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var listDirigentes = await _dirigenteService.GetAllWithInclude();
            var listVm = listDirigentes.Select(s => new DirigentePoliticoViewModel()
            {
                Id = s.Id,
                UsuarioId = s.UsuarioId,
                NombreUsuario = s.Usuario?.NombreUsuario ?? string.Empty,
                NombreCompletoUsuario = s.Usuario != null ? $"{s.Usuario.Nombre} {s.Usuario.Apellido}" : string.Empty,
                PartidoPoliticoId = s.PartidoPoliticoId,
                NombrePartido = s.PartidoPolitico?.Nombre ?? string.Empty,
                SiglasPartido = s.PartidoPolitico?.Siglas ?? string.Empty,
                Activo = s.Activo
            }).ToList();

            return View(listVm);
        }

        public async Task<IActionResult> Create()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            await CargarListasViewBag();
            return View(new SaveDirigentePoliticoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveDirigentePoliticoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid)
            {
                await CargarListasViewBag();
                return View(vm);
            }

            SaveDirigentePoliticoDto dto = new()
            {
                Id = 0,
                UsuarioId = vm.UsuarioId,
                PartidoPoliticoId = vm.PartidoPoliticoId,
                Activo = true
            };

            await _dirigenteService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            var listDirigentes = await _dirigenteService.GetAllWithInclude();
            var entity = listDirigentes.FirstOrDefault(d => d.Id == id);
            if (entity == null) return RedirectToAction("Index");

            DeleteDirigentePoliticoViewModel vm = new()
            {
                Id = entity.Id,
                NombreUsuario = entity.Usuario?.NombreUsuario ?? string.Empty,
                NombrePartido = entity.PartidoPolitico?.Nombre ?? string.Empty
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteDirigentePoliticoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.Administrador) return RedirectToAction("AccessDenied", "Login");

            await _dirigenteService.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }

        public async Task<DirigentePoliticoDto?> ObtenerDirigentePorUsuario(int usuarioId)
        {
            return await _dirigenteService.GetByUsuarioId(usuarioId);
        }

        private async Task CargarListasViewBag()
        {
            var usuarioService = HttpContext.RequestServices.GetRequiredService<IUsuarioService>();
            var partidoService = HttpContext.RequestServices.GetRequiredService<IPartidoPoliticoService>();

            var usuarios = await usuarioService.GetAll();
            var partidos = await partidoService.GetAllActivos();

            ViewBag.Usuarios = usuarios.Where(u => u.Rol == RolUsuario.DirigentePolitico && u.Activo).ToList();
            ViewBag.Partidos = partidos;
        }
    }
}