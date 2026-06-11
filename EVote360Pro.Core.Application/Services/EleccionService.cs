using EVote360Pro.Core.Application.Dtos.Eleccion;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Application.ViewModels.Eleccion;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Core.Application.Services
{
    public class EleccionService : IEleccionService
    {
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IVotoRepository _votoRepository;
        private readonly ICandidatoPuestoRepository _candidatoPuestoRepository;
        private readonly IPuestoElectivoRepository _puestoElectivoRepository;

        public EleccionService(
            IEleccionRepository eleccionRepository,
            IVotoRepository votoRepository,
            ICandidatoPuestoRepository candidatoPuestoRepository,
            IPuestoElectivoRepository puestoElectivoRepository)
        {
            _eleccionRepository = eleccionRepository;
            _votoRepository = votoRepository;
            _candidatoPuestoRepository = candidatoPuestoRepository;
            _puestoElectivoRepository = puestoElectivoRepository;
        }

        public async Task<EleccionDto?> AddAsync(SaveEleccionDto dto)
        {
            try
            {
                Eleccion entity = new()
                {
                    Id = 0,
                    Nombre = dto.Nombre,
                    FechaRealizacion = dto.FechaRealizacion,
                    Estado = EstadoEleccion.Pendiente,
                    Activo = true
                };

                Eleccion? returnEntity = await _eleccionRepository.AddAsync(entity);
                if (returnEntity == null) return null;

                return MapToDto(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _eleccionRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<EleccionDto>> GetAll()
        {
            try
            {
                var listEntities = await _eleccionRepository.GetAllList();
                return listEntities
                    .OrderByDescending(e => e.FechaRealizacion)
                    .Select(s => MapToDto(s))
                    .ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<EleccionDto?> GetById(int id)
        {
            try
            {
                var entity = await _eleccionRepository.GetById(id);
                if (entity == null) return null;
                return MapToDto(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EleccionDto?> GetEleccionActiva()
        {
            try
            {
                var entity = await _eleccionRepository.GetEleccionActiva();
                if (entity == null) return null;
                return MapToDto(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> ActivarEleccionAsync(int id)
        {
            try
            {
                bool existeActiva = await _eleccionRepository.ExisteEleccionActiva();
                if (existeActiva) return false;

                var entity = await _eleccionRepository.GetById(id);
                if (entity == null || entity.Estado != EstadoEleccion.Pendiente) return false;

                var puestosActivos = await _puestoElectivoRepository.GetAllActivos();
                if (puestosActivos.Count == 0) return false;

                foreach (var puesto in puestosActivos)
                {
                    var asignaciones = await _candidatoPuestoRepository.GetByPartido(0);
                    // validacion de configuracion electoral completa se hace en el controller
                }

                entity.Estado = EstadoEleccion.Activa;
                await _eleccionRepository.UpdateAsync(entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> FinalizarEleccionAsync(int id)
        {
            try
            {
                var entity = await _eleccionRepository.GetById(id);
                if (entity == null || entity.Estado != EstadoEleccion.Activa) return false;

                entity.Estado = EstadoEleccion.Finalizada;
                await _eleccionRepository.UpdateAsync(entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ResultadosEleccionViewModel?> GetResultados(int eleccionId)
        {
            try
            {
                var eleccion = await _eleccionRepository.GetById(eleccionId);
                if (eleccion == null || eleccion.Estado != EstadoEleccion.Finalizada) return null;

                var votos = await _votoRepository.GetByEleccion(eleccionId);
                var puestos = await _puestoElectivoRepository.GetAllActivos();

                var resultado = new ResultadosEleccionViewModel()
                {
                    EleccionId = eleccionId,
                    NombreEleccion = eleccion.Nombre,
                    ResultadosPorPuesto = new List<ResultadoPuestoViewModel>()
                };

                foreach (var puesto in puestos)
                {
                    var votosPuesto = votos
                        .Where(v => v.PuestoElectivoId == puesto.Id && v.Finalizado)
                        .ToList();

                    int totalVotos = votosPuesto.Count;

                    var agrupados = votosPuesto
                        .GroupBy(v => new { v.CandidatoId, v.EsNinguno })
                        .Select(g => new ResultadoCandidatoViewModel()
                        {
                            CandidatoId = g.Key.CandidatoId,
                            EsNinguno = g.Key.EsNinguno,
                            NombreCandidato = g.Key.EsNinguno
                                ? "Ninguno"
                                : (g.First().Candidato != null
                                    ? $"{g.First().Candidato!.Nombre} {g.First().Candidato!.Apellido}"
                                    : "Desconocido"),
                            NombrePartido = g.Key.EsNinguno
                                ? string.Empty
                                : (g.First().Candidato?.PartidoPolitico?.Nombre ?? string.Empty),
                            TotalVotos = g.Count(),
                            Porcentaje = totalVotos > 0
                                ? Math.Round((double)g.Count() / totalVotos * 100, 2)
                                : 0
                        })
                        .OrderByDescending(r => r.TotalVotos)
                        .ToList();

                    int maxVotos = agrupados.Where(a => !a.EsNinguno).Select(a => a.TotalVotos).DefaultIfEmpty(0).Max();
                    var ganadores = agrupados.Where(a => !a.EsNinguno && a.TotalVotos == maxVotos).ToList();
                    bool hayEmpate = ganadores.Count > 1;

                    resultado.ResultadosPorPuesto.Add(new ResultadoPuestoViewModel()
                    {
                        PuestoElectivoId = puesto.Id,
                        NombrePuesto = puesto.Nombre,
                        ResultadosCandidatos = agrupados,
                        HayEmpate = hayEmpate,
                        GanadorNombre = hayEmpate ? null : ganadores.FirstOrDefault()?.NombreCandidato
                    });
                }

                return resultado;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<int>> GetAnosConElecciones()
        {
            try
            {
                return await _eleccionRepository.GetAnosConElecciones();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<EleccionDto>> GetByAno(int ano)
        {
            try
            {
                var listEntities = await _eleccionRepository.GetByAno(ano);
                return listEntities.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        private static EleccionDto MapToDto(Eleccion entity)
        {
            return new EleccionDto()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                FechaRealizacion = entity.FechaRealizacion,
                Estado = entity.Estado,
                Activo = entity.Activo
            };
        }
    }
}