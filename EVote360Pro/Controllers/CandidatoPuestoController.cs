using EVote360Pro.Core.Application.Dtos.CandidatoPuesto;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.CandidatoPuesto;
using EVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class CandidatoPuestoController : Controller
    {
        private readonly ICandidatoPuestoService _candidatoPuestoService;
        private readonly ICandidatoService _candidatoService;
        private readonly IPuestoElectivoService _puestoElectivoService;
        private readonly IDirigentePoliticoService _dirigenteService;
        private readonly IEleccionService _eleccionService;
        private readonly IUserSession _userSession;

        public CandidatoPuestoController(
            ICandidatoPuestoService candidatoPuestoService,
            ICandidatoService candidatoService,
            IPuestoElectivoService puestoElectivoService,
            IDirigentePoliticoService dirigenteService,
            IEleccionService eleccionService,
            IUserSession userSession)
        {
            _candidatoPuestoService = candidatoPuestoService;
            _candidatoService = candidatoService;
            _puestoElectivoService = puestoElectivoService;
            _dirigenteService = dirigenteService;
            _eleccionService = eleccionService;
            _userSession = userSession;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.DirigentePolitico) return RedirectToAction("AccessDenied", "Login");

            var dirigente = await _dirigenteService.GetByUsuarioId(usuario.Id);
            if (dirigente == null) return RedirectToAction("AccessDenied", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            ViewBag.HayEleccionActiva = eleccionActiva != null;

            var listAsignaciones = await _candidatoPuestoService.GetByPartido(dirigente.PartidoPoliticoId);
            var listVm = listAsignaciones.Select(s => new CandidatoPuestoViewModel()
            {
                Id = s.Id,
                CandidatoId = s.CandidatoId,
                NombreCandidato = s.Candidato != null ? $"{s.Candidato.Nombre} {s.Candidato.Apellido}" : string.Empty,
                PuestoElectivoId = s.PuestoElectivoId,
                NombrePuesto = s.PuestoElectivo?.Nombre ?? string.Empty,
                PartidoPoliticoId = s.PartidoPoliticoId,
                NombrePartido = s.PartidoPolitico?.Nombre ?? string.Empty,
                EsCandidatoAliado = s.Candidato?.PartidoPoliticoId != dirigente.PartidoPoliticoId,
                Activo = s.Activo
            }).ToList();

            return View(listVm);
        }

        public async Task<IActionResult> Create()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.DirigentePolitico) return RedirectToAction("AccessDenied", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se pueden crear asignaciones mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            var dirigente = await _dirigenteService.GetByUsuarioId(usuario.Id);
            if (dirigente == null) return RedirectToAction("AccessDenied", "Login");

            var candidatosPropios = await _candidatoService.GetByPartido(dirigente.PartidoPoliticoId);
            var puestos = await _puestoElectivoService.GetAllActivos();

            ViewBag.Candidatos = candidatosPropios.Where(c => c.Activo).ToList();
            ViewBag.Puestos = puestos;

            return View(new SaveCandidatoPuestoViewModel()
            {
                PartidoPoliticoId = dirigente.PartidoPoliticoId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCandidatoPuestoViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.DirigentePolitico) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid)
            {
                var dirigente2 = await _dirigenteService.GetByUsuarioId(usuario.Id);
                if (dirigente2 != null)
                {
                    ViewBag.Candidatos = (await _candidatoService.GetByPartido(dirigente2.PartidoPoliticoId))
                        .Where(c => c.Activo).ToList();
                }
                ViewBag.Puestos = await _puestoElectivoService.GetAllActivos();
                return View(vm);
            }

            SaveCandidatoPuestoDto dto = new()
            {
                Id = 0,
                CandidatoId = vm.CandidatoId,
                PuestoElectivoId = vm.PuestoElectivoId,
                PartidoPoliticoId = vm.PartidoPoliticoId,
                Activo = true
            };

            await _candidatoPuestoService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.DirigentePolitico) return RedirectToAction("AccessDenied", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se pueden eliminar asignaciones mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            await _candidatoPuestoService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}