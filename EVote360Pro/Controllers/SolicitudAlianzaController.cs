using EVote360Pro.Core.Application.Dtos.SolicitudAlianza;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.SolicitudAlianza;
using EVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class SolicitudAlianzaController : Controller
    {
        private readonly ISolicitudAlianzaService _solicitudService;
        private readonly IPartidoPoliticoService _partidoService;
        private readonly IDirigentePoliticoService _dirigenteService;
        private readonly IEleccionService _eleccionService;
        private readonly IUserSession _userSession;

        public SolicitudAlianzaController(
            ISolicitudAlianzaService solicitudService,
            IPartidoPoliticoService partidoService,
            IDirigentePoliticoService dirigenteService,
            IEleccionService eleccionService,
            IUserSession userSession)
        {
            _solicitudService = solicitudService;
            _partidoService = partidoService;
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

            var pendientes = await _solicitudService.GetSolicitudesPendientesParaPartido(dirigente.PartidoPoliticoId);
            var realizadas = await _solicitudService.GetSolicitudesRealizadasPorPartido(dirigente.PartidoPoliticoId);
            var alianzas = await _solicitudService.GetAlianzasVigentes(dirigente.PartidoPoliticoId);

            ViewBag.SolicitudesPendientes = pendientes.Select(s => new SolicitudAlianzaViewModel()
            {
                Id = s.Id,
                PartidoSolicitanteId = s.PartidoSolicitanteId,
                NombrePartidoSolicitante = s.PartidoSolicitante?.Nombre ?? string.Empty,
                PartidoReceptorId = s.PartidoReceptorId,
                NombrePartidoReceptor = s.PartidoReceptor?.Nombre ?? string.Empty,
                FechaSolicitud = s.FechaSolicitud,
                Estado = s.Estado
            }).ToList();

            ViewBag.SolicitudesRealizadas = realizadas.Select(s => new SolicitudAlianzaViewModel()
            {
                Id = s.Id,
                PartidoSolicitanteId = s.PartidoSolicitanteId,
                NombrePartidoSolicitante = s.PartidoSolicitante?.Nombre ?? string.Empty,
                PartidoReceptorId = s.PartidoReceptorId,
                NombrePartidoReceptor = s.PartidoReceptor?.Nombre ?? string.Empty,
                FechaSolicitud = s.FechaSolicitud,
                FechaRespuesta = s.FechaRespuesta,
                Estado = s.Estado
            }).ToList();

            ViewBag.AlianzasVigentes = alianzas.Select(s => new SolicitudAlianzaViewModel()
            {
                Id = s.Id,
                PartidoSolicitanteId = s.PartidoSolicitanteId,
                NombrePartidoSolicitante = s.PartidoSolicitante?.Nombre ?? string.Empty,
                PartidoReceptorId = s.PartidoReceptorId,
                NombrePartidoReceptor = s.PartidoReceptor?.Nombre ?? string.Empty,
                FechaSolicitud = s.FechaSolicitud,
                FechaRespuesta = s.FechaRespuesta,
                Estado = s.Estado
            }).ToList();

            ViewBag.PartidoActualId = dirigente.PartidoPoliticoId;

            return View();
        }

        public async Task<IActionResult> Create()
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.DirigentePolitico) return RedirectToAction("AccessDenied", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se pueden crear alianzas mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            var dirigente = await _dirigenteService.GetByUsuarioId(usuario.Id);
            if (dirigente == null) return RedirectToAction("AccessDenied", "Login");

            var partidos = await _partidoService.GetAllActivos();
            ViewBag.Partidos = partidos.Where(p => p.Id != dirigente.PartidoPoliticoId).ToList();

            return View(new SaveSolicitudAlianzaViewModel()
            {
                PartidoSolicitanteId = dirigente.PartidoPoliticoId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveSolicitudAlianzaViewModel vm)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");
            if (usuario.Rol != RolUsuario.DirigentePolitico) return RedirectToAction("AccessDenied", "Login");

            if (!ModelState.IsValid)
            {
                var partidos = await _partidoService.GetAllActivos();
                ViewBag.Partidos = partidos.Where(p => p.Id != vm.PartidoSolicitanteId).ToList();
                return View(vm);
            }

            SaveSolicitudAlianzaDto dto = new()
            {
                Id = 0,
                PartidoSolicitanteId = vm.PartidoSolicitanteId,
                PartidoReceptorId = vm.PartidoReceptorId,
                Activo = true
            };

            var resultado = await _solicitudService.CrearSolicitudAsync(dto);
            if (resultado == null)
            {
                TempData["Error"] = "No se pudo crear la solicitud. Verifique que no exista una solicitud pendiente o alianza vigente.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Aceptar(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            await _solicitudService.AceptarSolicitudAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Rechazar(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            await _solicitudService.RechazarSolicitudAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarSolicitud(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se pueden eliminar solicitudes mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            bool resultado = await _solicitudService.EliminarSolicitudAsync(id);
            if (!resultado)
            {
                TempData["Error"] = "No se puede eliminar esta solicitud.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarAlianza(int id)
        {
            var usuario = _userSession.GetUser();
            if (usuario == null) return RedirectToAction("Index", "Login");

            var eleccionActiva = await _eleccionService.GetEleccionActiva();
            if (eleccionActiva != null)
            {
                TempData["Error"] = "No se pueden eliminar alianzas mientras hay una elección activa.";
                return RedirectToAction("Index");
            }

            bool resultado = await _solicitudService.EliminarAlianzaAsync(id);
            if (!resultado)
            {
                TempData["Error"] = "No se puede eliminar esta alianza. Puede que existan candidatos aliados asignados.";
            }

            return RedirectToAction("Index");
        }
    }
}