using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class DirigentHomeController : Controller
    {
        private readonly IUserSession _userSession;
        private readonly IDirigentePoliticoService _dirigenteService;
        private readonly ICandidatoService _candidatoService;
        private readonly ICandidatoPuestoService _candidatoPuestoService;
        private readonly ISolicitudAlianzaService _solicitudService;
        private readonly IEleccionService _eleccionService;

        public DirigentHomeController(
            IUserSession userSession,
            IDirigentePoliticoService dirigenteService,
            ICandidatoService candidatoService,
            ICandidatoPuestoService candidatoPuestoService,
            ISolicitudAlianzaService solicitudService,
            IEleccionService eleccionService)
        {
            _userSession = userSession;
            _dirigenteService = dirigenteService;
            _candidatoService = candidatoService;
            _candidatoPuestoService = candidatoPuestoService;
            _solicitudService = solicitudService;
            _eleccionService = eleccionService;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.DirigentePolitico) return RedirectToAction("AccessDenied", "Login");

            var dirigente = await _dirigenteService.GetByUsuarioId(usuario.Id);
            if (dirigente == null || !dirigente.Activo)
            {
                TempData["Error"] = "No tiene un partido político asignado o su cuenta está inactiva.";
                return RedirectToAction("AccessDenied", "Login");
            }

            if (dirigente.PartidoPolitico == null || !dirigente.PartidoPolitico.Activo)
            {
                TempData["Error"] = "Su partido político no está activo.";
                return RedirectToAction("AccessDenied", "Login");
            }

            var candidatos = await _candidatoService.GetByPartido(dirigente.PartidoPoliticoId);
            var asignaciones = await _candidatoPuestoService.GetByPartido(dirigente.PartidoPoliticoId);
            var alianzas = await _solicitudService.GetAlianzasVigentes(dirigente.PartidoPoliticoId);
            var solicitudesPendientes = await _solicitudService.GetSolicitudesPendientesParaPartido(dirigente.PartidoPoliticoId);
            var eleccionActiva = await _eleccionService.GetEleccionActiva();

            ViewBag.Dirigente = dirigente;
            ViewBag.TotalCandidatos = candidatos.Count;
            ViewBag.TotalCandidatosActivos = candidatos.Count(c => c.Activo);
            ViewBag.TotalAsignaciones = asignaciones.Count;
            ViewBag.TotalAlianzas = alianzas.Count;
            ViewBag.TotalSolicitudesPendientes = solicitudesPendientes.Count;
            ViewBag.HayEleccionActiva = eleccionActiva != null;
            ViewBag.NombreEleccionActiva = eleccionActiva?.Nombre ?? string.Empty;

            return View();
        }
    }
}