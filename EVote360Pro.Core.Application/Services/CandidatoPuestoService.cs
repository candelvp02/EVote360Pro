using EVote360Pro.Core.Application.Dtos.CandidatoPuesto;
using EVote360Pro.Core.Application.Dtos.Candidato;
using EVote360Pro.Core.Application.Dtos.PartidoPolitico;
using EVote360Pro.Core.Application.Dtos.PuestoElectivo;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Core.Application.Services
{
    public class CandidatoPuestoService : ICandidatoPuestoService
    {
        private readonly ICandidatoPuestoRepository _candidatoPuestoRepository;

        public CandidatoPuestoService(ICandidatoPuestoRepository candidatoPuestoRepository)
        {
            _candidatoPuestoRepository = candidatoPuestoRepository;
        }

        public async Task<CandidatoPuestoDto?> AddAsync(SaveCandidatoPuestoDto dto)
        {
            try
            {
                CandidatoPuesto entity = new()
                {
                    Id = 0,
                    CandidatoId = dto.CandidatoId,
                    PuestoElectivoId = dto.PuestoElectivoId,
                    PartidoPoliticoId = dto.PartidoPoliticoId,
                    Activo = true
                };

                CandidatoPuesto? returnEntity = await _candidatoPuestoRepository.AddAsync(entity);
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
                await _candidatoPuestoRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<CandidatoPuestoDto>> GetByPartido(int partidoId)
        {
            try
            {
                var listEntitiesQuery = _candidatoPuestoRepository
                    .GetAllQueryWithInclude(["Candidato", "Candidato.PartidoPolitico", "PuestoElectivo", "PartidoPolitico"])
                    .Where(cp => cp.PartidoPoliticoId == partidoId);

                return await listEntitiesQuery.Select(s => new CandidatoPuestoDto()
                {
                    Id = s.Id,
                    CandidatoId = s.CandidatoId,
                    PuestoElectivoId = s.PuestoElectivoId,
                    PartidoPoliticoId = s.PartidoPoliticoId,
                    Activo = s.Activo,
                    Candidato = s.Candidato == null ? null : new CandidatoDto()
                    {
                        Id = s.Candidato.Id,
                        Nombre = s.Candidato.Nombre,
                        Apellido = s.Candidato.Apellido,
                        FotoUrl = s.Candidato.FotoUrl,
                        PartidoPoliticoId = s.Candidato.PartidoPoliticoId,
                        Activo = s.Candidato.Activo
                    },
                    PuestoElectivo = s.PuestoElectivo == null ? null : new PuestoElectivoDto()
                    {
                        Id = s.PuestoElectivo.Id,
                        Nombre = s.PuestoElectivo.Nombre,
                        Descripcion = s.PuestoElectivo.Descripcion,
                        Activo = s.PuestoElectivo.Activo
                    },
                    PartidoPolitico = s.PartidoPolitico == null ? null : new PartidoPoliticoDto()
                    {
                        Id = s.PartidoPolitico.Id,
                        Nombre = s.PartidoPolitico.Nombre,
                        Siglas = s.PartidoPolitico.Siglas,
                        LogoUrl = s.PartidoPolitico.LogoUrl,
                        Activo = s.PartidoPolitico.Activo
                    }
                }).ToListAsync();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<CandidatoPuestoDto?> GetById(int id)
        {
            try
            {
                var entity = await _candidatoPuestoRepository.GetById(id);
                if (entity == null) return null;
                return MapToDto(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static CandidatoPuestoDto MapToDto(CandidatoPuesto entity)
        {
            return new CandidatoPuestoDto()
            {
                Id = entity.Id,
                CandidatoId = entity.CandidatoId,
                PuestoElectivoId = entity.PuestoElectivoId,
                PartidoPoliticoId = entity.PartidoPoliticoId,
                Activo = entity.Activo
            };
        }
    }
}