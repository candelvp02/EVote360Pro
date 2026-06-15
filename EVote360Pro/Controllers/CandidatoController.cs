using EVote360Pro.Core.Application.Dtos.Candidato;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.Candidato;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class CandidatoController : Controller
    {
        private readonly ICandidatoService _candidatoService;
        private readonly IPartidoPoliticoService _partidoPoliticoService;
        private readonly IEleccionService _eleccionService;
        private readonly IUserSession _userSession;

        public CandidatoController(ICandidatoService candidatoService, IPartidoPoliticoService partidoPoliticoService, IEleccionService eleccionService, IUserSession userSession)
        {
            _candidatoService = candidatoService;
            _partidoPoliticoService = partidoPoliticoService;
            _eleccionService = eleccionService;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            ViewBag.HayEleccionActiva = eleccionActiva != null;

            List<CandidatoViewModel> listVm = new();

            if (usuario.Rol == RolUsuario.Administrador)
            {
                var listCandidatos = await _candidatoService.GetAll();
                listVm = listCandidatos.Select(s => new CandidatoViewModel()
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Apellido = s.Apellido,
                    FotoUrl = s.FotoUrl,
                    PartidoPoliticoId = s.PartidoPoliticoId,
                    NombrePartido = s.PartidoPolitico?.Nombre ?? string.Empty,
                    Activo = s.Activo
                }).ToList();
            }
            else if (usuario.Rol == RolUsuario.DirigentePolitico)
            {
                var dirigente = await ObtenerDirigenteActual(usuario.Id);
                if (dirigente == null) return RedirectToAction("AccessDenied", "Login");

                var listCandidatos = await _candidatoService.GetByPartido(dirigente.PartidoPoliticoId);
                listVm = listCandidatos.Select(s => new CandidatoViewModel()
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Apellido = s.Apellido,
                    FotoUrl = s.FotoUrl,
                    PartidoPoliticoId = s.PartidoPoliticoId,
                    Activo = s.Activo
                }).ToList();
            }

            return View(listVm);
        }

        public async Task<IActionResult> Create()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se pueden crear candidatos mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            var vm = new SaveCandidatoViewModel();

            if (usuario.Rol == RolUsuario.DirigentePolitico)
            {
                var dirigente = await ObtenerDirigenteActual(usuario.Id);
                if (dirigente == null) return RedirectToAction("AccessDenied", "Login");
                vm.PartidoPoliticoId = dirigente.PartidoPoliticoId;
            }
            else
            {
                ViewBag.Partidos = await _partidoPoliticoService.GetAllActivos();
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCandidatoViewModel vm, IFormFile? fotoFile)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                ViewBag.Partidos = await _partidoPoliticoService.GetAllActivos();
                return View(vm);
            }

            SaveCandidatoDto dto = new()
            {
                Id = 0,
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                FotoUrl = string.Empty,
                PartidoPoliticoId = vm.PartidoPoliticoId,
                Activo = true
            };

            var returnDto = await _candidatoService.AddAsync(dto);

            if (returnDto != null && fotoFile != null)
            {
                string fotoUrl = FileManager.Upload(fotoFile, returnDto.Id, "Candidatos");
                dto.Id = returnDto.Id;
                dto.FotoUrl = fotoUrl;
                await _candidatoService.UpdateAsync(dto);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se pueden editar candidatos mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            var entity = await _candidatoService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            SaveCandidatoViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                FotoUrl = entity.FotoUrl,
                PartidoPoliticoId = entity.PartidoPoliticoId,
                Activo = entity.Activo
            };

            ViewBag.Partidos = await _partidoPoliticoService.GetAllActivos();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCandidatoViewModel vm, IFormFile? fotoFile)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                ViewBag.Partidos = await _partidoPoliticoService.GetAllActivos();
                return View(vm);
            }

            string fotoUrl = vm.FotoUrl;
            if (fotoFile != null)
            {
                fotoUrl = FileManager.Upload(fotoFile, vm.Id, "Candidatos");
            }

            SaveCandidatoDto dto = new()
            {
                Id = vm.Id,
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                FotoUrl = fotoUrl,
                PartidoPoliticoId = vm.PartidoPoliticoId,
                Activo = vm.Activo
            };

            await _candidatoService.UpdateAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            var entity = await _candidatoService.GetById(id);
            if (entity == null) return RedirectToAction("Index");

            DeleteCandidatoViewModel vm = new()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCandidatoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            await _candidatoService.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se puede cambiar el estado mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            await _candidatoService.CambiarEstadoAsync(id);
            return RedirectToAction("Index");
        }

        private async Task<Core.Application.Dtos.DirigentePolitico.DirigentePoliticoDto?> ObtenerDirigenteActual(int usuarioId)
        {
            var dirigentes = await new DirigentePoliticoController(
                HttpContext.RequestServices.GetRequiredService<IDirigentePoliticoService>(),
                _userSession).ObtenerDirigentePorUsuario(usuarioId);
            return dirigentes;
        }
    }
}