using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.Voto;
using EVote360Pro.Infrastructure.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace EVote360Pro.Controllers
{
    public class VotacionController : Controller
    {
        private readonly IVotacionService _votacionService;
        private readonly ICiudadanoService _ciudadanoService;
        private readonly IEleccionService _eleccionService;
        private readonly EmailService _emailService;

        public VotacionController(
            IVotacionService votacionService,
            ICiudadanoService ciudadanoService,
            IEleccionService eleccionService,
            EmailService emailService)
        {
            _votacionService = votacionService;
            _ciudadanoService = ciudadanoService;
            _eleccionService = eleccionService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View(new IniciarVotacionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(IniciarVotacionViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var (exito, mensaje, ciudadanoId, eleccionId) = await _votacionService.ValidarInicioVotacionAsync(vm.NumeroDocumento);

            if (!exito)
            {
                ModelState.AddModelError("validacion", mensaje);
                return View(vm);
            }

            bool codigoGenerado = await _votacionService.GenerarYEnviarCodigoAsync(ciudadanoId, eleccionId);

            if (codigoGenerado)
            {
                var ciudadano = await _ciudadanoService.GetById(ciudadanoId);
                var codigoRepo = HttpContext.RequestServices
                    .GetRequiredService<Core.Domain.Interfaces.ICodigoVerificacionRepository>();
                var codigoVigente = await codigoRepo.GetUltimoVigente(ciudadanoId, eleccionId);

                if (ciudadano != null && codigoVigente != null)
                {
                    await _emailService.SendCodigoVerificacionAsync(
                        ciudadano.CorreoElectronico,
                        $"{ciudadano.Nombre} {ciudadano.Apellido}",
                        codigoVigente.Codigo);
                }
            }

            return RedirectToAction("VerificarCodigo", new { ciudadanoId, eleccionId });
        }

        public IActionResult VerificarCodigo(int ciudadanoId, int eleccionId)
        {
            return View(new VerificarCodigoViewModel()
            {
                CiudadanoId = ciudadanoId,
                EleccionId = eleccionId
            });
        }

        [HttpPost]
        public async Task<IActionResult> VerificarCodigo(VerificarCodigoViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            bool valido = await _votacionService.ValidarCodigoAsync(vm.CiudadanoId, vm.EleccionId, vm.Codigo);

            if (!valido)
            {
                ModelState.AddModelError("codigoInvalido", "El código es incorrecto, ha expirado o ya fue utilizado.");
                return View(vm);
            }

            return RedirectToAction("Votar", new { ciudadanoId = vm.CiudadanoId, eleccionId = vm.EleccionId });
        }

        public async Task<IActionResult> Votar(int ciudadanoId, int eleccionId)
        {
            var vm = await _votacionService.GetPuestosParaVotarAsync(ciudadanoId, eleccionId);
            if (vm == null) return RedirectToAction("Index");

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarVoto(int ciudadanoId, int eleccionId, Dictionary<int, int?> selecciones)
        {
            if (selecciones == null || selecciones.Count == 0)
            {
                TempData["Error"] = "Debe seleccionar una opción para todos los puestos electivos.";
                return RedirectToAction("Votar", new { ciudadanoId, eleccionId });
            }

            bool resultado = await _votacionService.RegistrarVotosAsync(ciudadanoId, eleccionId, selecciones);

            if (!resultado)
            {
                TempData["Error"] = "Ocurrió un error al registrar su voto. Intente nuevamente.";
                return RedirectToAction("Votar", new { ciudadanoId, eleccionId });
            }

            var ciudadano = await _ciudadanoService.GetById(ciudadanoId);
            var eleccion = await _eleccionService.GetById(eleccionId);
            var resumenVotos = await _votacionService.GetResumenVotosAsync(ciudadanoId, eleccionId);

            if (ciudadano != null && eleccion != null)
            {
                var votosResumen = resumenVotos.Select(v => (v.NombrePuesto, v.NombreCandidato)).ToList();
                await _emailService.SendResumenVotacionAsync(
                    ciudadano.CorreoElectronico,
                    $"{ciudadano.Nombre} {ciudadano.Apellido}",
                    eleccion.Nombre,
                    votosResumen);
            }

            return RedirectToAction("Confirmacion");
        }

        public IActionResult Confirmacion()
        {
            return View();
        }
    }
}