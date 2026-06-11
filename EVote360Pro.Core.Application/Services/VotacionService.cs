using EVote360Pro.Core.Application.Dtos.Voto;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.Voto;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Core.Application.Services
{
    public class VotacionService : IVotacionService
    {
        private readonly ICiudadanoRepository _ciudadanoRepository;
        private readonly IEleccionRepository _eleccionRepository;
        private readonly ICodigoVerificacionRepository _codigoRepository;
        private readonly IVotoRepository _votoRepository;
        private readonly ICandidatoPuestoRepository _candidatoPuestoRepository;
        private readonly IPuestoElectivoRepository _puestoElectivoRepository;

        public VotacionService(
            ICiudadanoRepository ciudadanoRepository,
            IEleccionRepository eleccionRepository,
            ICodigoVerificacionRepository codigoRepository,
            IVotoRepository votoRepository,
            ICandidatoPuestoRepository candidatoPuestoRepository,
            IPuestoElectivoRepository puestoElectivoRepository)
        {
            _ciudadanoRepository = ciudadanoRepository;
            _eleccionRepository = eleccionRepository;
            _codigoRepository = codigoRepository;
            _votoRepository = votoRepository;
            _candidatoPuestoRepository = candidatoPuestoRepository;
            _puestoElectivoRepository = puestoElectivoRepository;
        }

        public async Task<(bool exito, string mensaje, int ciudadanoId, int eleccionId)> ValidarInicioVotacionAsync(string numeroDocumento)
        {
            try
            {
                var eleccionActiva = await _eleccionRepository.GetEleccionActiva();
                if (eleccionActiva == null)
                    return (false, "No existe una elección activa en este momento.", 0, 0);

                var ciudadano = await _ciudadanoRepository.GetByNumeroDocumento(numeroDocumento);
                if (ciudadano == null)
                    return (false, "El número de documento no está registrado.", 0, 0);

                if (!ciudadano.Activo)
                    return (false, "El ciudadano no se encuentra activo.", 0, 0);

                bool yaVoto = await _ciudadanoRepository.YaVotoEnEleccion(ciudadano.Id, eleccionActiva.Id);
                if (yaVoto)
                    return (false, "El ciudadano ya ejerció su voto en esta elección.", 0, 0);

                return (true, "Validación exitosa.", ciudadano.Id, eleccionActiva.Id);
            }
            catch (Exception)
            {
                return (false, "Error al validar el inicio de votación.", 0, 0);
            }
        }

        public async Task<bool> GenerarYEnviarCodigoAsync(int ciudadanoId, int eleccionId)
        {
            try
            {
                string codigo = new Random().Next(100000, 999999).ToString();

                CodigoVerificacion entity = new()
                {
                    Id = 0,
                    CiudadanoId = ciudadanoId,
                    EleccionId = eleccionId,
                    Codigo = codigo,
                    FechaGeneracion = DateTime.Now,
                    FechaExpiracion = DateTime.Now.AddMinutes(10),
                    Usado = false,
                    Activo = true
                };

                await _codigoRepository.AddAsync(entity);
                // El envio de correo se hace desde el controller usando IEmailService
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ValidarCodigoAsync(int ciudadanoId, int eleccionId, string codigo)
        {
            try
            {
                var codigoVigente = await _codigoRepository.GetUltimoVigente(ciudadanoId, eleccionId);

                if (codigoVigente == null) return false;
                if (codigoVigente.Usado) return false;
                if (codigoVigente.FechaExpiracion < DateTime.Now) return false;
                if (codigoVigente.Codigo != codigo) return false;

                codigoVigente.Usado = true;
                await _codigoRepository.UpdateAsync(codigoVigente.Id, codigoVigente);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<SeleccionVotoViewModel?> GetPuestosParaVotarAsync(int ciudadanoId, int eleccionId)
        {
            try
            {
                var ciudadano = await _ciudadanoRepository.GetById(ciudadanoId);
                if (ciudadano == null) return null;

                var puestosActivos = await _puestoElectivoRepository.GetAllActivos();

                var puestosVotacion = new List<PuestoVotacionViewModel>();

                foreach (var puesto in puestosActivos)
                {
                    var asignacionesQuery = _candidatoPuestoRepository
                        .GetAllQueryWithInclude(["Candidato", "Candidato.PartidoPolitico", "PartidoPolitico"])
                        .Where(cp => cp.PuestoElectivoId == puesto.Id);

                    var asignaciones = await asignacionesQuery.ToListAsync();

                    var candidatosVotacion = asignaciones
                        .Where(a => a.Candidato != null && a.Candidato.Activo)
                        .Select(a => new CandidatoVotacionViewModel()
                        {
                            CandidatoId = a.CandidatoId,
                            NombreCompleto = $"{a.Candidato!.Nombre} {a.Candidato.Apellido}",
                            FotoUrl = a.Candidato.FotoUrl,
                            NombrePartido = a.Candidato.PartidoPolitico?.Nombre ?? string.Empty,
                            LogoPartido = a.Candidato.PartidoPolitico?.LogoUrl ?? string.Empty
                        }).ToList();

                    puestosVotacion.Add(new PuestoVotacionViewModel()
                    {
                        PuestoElectivoId = puesto.Id,
                        NombrePuesto = puesto.Nombre,
                        Candidatos = candidatosVotacion
                    });
                }

                return new SeleccionVotoViewModel()
                {
                    CiudadanoId = ciudadanoId,
                    EleccionId = eleccionId,
                    NombreCiudadano = $"{ciudadano.Nombre} {ciudadano.Apellido}",
                    PuestosElectivos = puestosVotacion
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> RegistrarVotosAsync(int ciudadanoId, int eleccionId, Dictionary<int, int?> selecciones)
        {
            try
            {
                foreach (var seleccion in selecciones)
                {
                    int puestoId = seleccion.Key;
                    int? candidatoId = seleccion.Value;
                    bool esNinguno = candidatoId == null;

                    Voto voto = new()
                    {
                        Id = 0,
                        CiudadanoId = ciudadanoId,
                        EleccionId = eleccionId,
                        PuestoElectivoId = puestoId,
                        CandidatoId = candidatoId,
                        EsNinguno = esNinguno,
                        Finalizado = true,
                        Activo = true
                    };

                    await _votoRepository.AddAsync(voto);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<VotoDto>> GetResumenVotosAsync(int ciudadanoId, int eleccionId)
        {
            try
            {
                var votos = await _votoRepository.GetByEleccionYCiudadano(eleccionId, ciudadanoId);

                var votosQuery = _votoRepository
                    .GetAllQueryWithInclude(["PuestoElectivo", "Candidato", "Candidato.PartidoPolitico"])
                    .Where(v => v.CiudadanoId == ciudadanoId && v.EleccionId == eleccionId && v.Finalizado);

                return await votosQuery.Select(v => new VotoDto()
                {
                    Id = v.Id,
                    CiudadanoId = v.CiudadanoId,
                    EleccionId = v.EleccionId,
                    PuestoElectivoId = v.PuestoElectivoId,
                    NombrePuesto = v.PuestoElectivo != null ? v.PuestoElectivo.Nombre : string.Empty,
                    CandidatoId = v.CandidatoId,
                    NombreCandidato = v.EsNinguno
                        ? "Ninguno"
                        : (v.Candidato != null ? $"{v.Candidato.Nombre} {v.Candidato.Apellido}" : string.Empty),
                    NombrePartido = v.EsNinguno
                        ? string.Empty
                        : (v.Candidato != null && v.Candidato.PartidoPolitico != null
                            ? v.Candidato.PartidoPolitico.Nombre
                            : string.Empty),
                    EsNinguno = v.EsNinguno,
                    Finalizado = v.Finalizado,
                    Activo = v.Activo
                }).ToListAsync();
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}