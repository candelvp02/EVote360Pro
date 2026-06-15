using EVote360Pro.Core.Application.Dtos.Candidato;
using EVote360Pro.Core.Application.Dtos.PartidoPolitico;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Core.Application.Services
{
    public class CandidatoService : ICandidatoService
    {
        private readonly ICandidatoRepository _candidatoRepository;

        public CandidatoService(ICandidatoRepository candidatoRepository)
        {
            _candidatoRepository = candidatoRepository;
        }

        public async Task<CandidatoDto?> AddAsync(SaveCandidatoDto dto)
        {
            try
            {
                Candidato entity = new()
                {
                    Id = 0,
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    FotoUrl = dto.FotoUrl,
                    PartidoPoliticoId = dto.PartidoPoliticoId,
                    Activo = true
                };

                Candidato? returnEntity = await _candidatoRepository.AddAsync(entity);
                if (returnEntity == null) return null;

                return MapToDto(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CandidatoDto?> UpdateAsync(SaveCandidatoDto dto)
        {
            try
            {
                var existing = await _candidatoRepository.GetById(dto.Id);
                if (existing == null) return null;

                existing.Nombre = dto.Nombre;
                existing.Apellido = dto.Apellido;
                existing.FotoUrl = dto.FotoUrl;
                existing.Activo = dto.Activo;

                Candidato? returnEntity = await _candidatoRepository.UpdateAsync(existing.Id, existing);
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
                await _candidatoRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CambiarEstadoAsync(int id)
        {
            try
            {
                var entity = await _candidatoRepository.GetById(id);
                if (entity == null) return false;

                // verificar que no tenga puesto asignado al momento de inactivar
                if (entity.Activo)
                {
                    bool tienePuesto = await _candidatoRepository.TienePuestoAsignado(id);
                    if (tienePuesto)
                        return false;
                }

                entity.Activo = !entity.Activo;
                await _candidatoRepository.UpdateAsync(entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<CandidatoDto>> GetAll()
        {
            try
            {
                var listEntitiesQuery = _candidatoRepository.GetAllQueryWithInclude(["PartidoPolitico"]);
                return await listEntitiesQuery.Select(s => new CandidatoDto()
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Apellido = s.Apellido,
                    FotoUrl = s.FotoUrl,
                    PartidoPoliticoId = s.PartidoPoliticoId,
                    PartidoPolitico = s.PartidoPolitico == null ? null : new PartidoPoliticoDto()
                    {
                        Id = s.PartidoPolitico.Id,
                        Nombre = s.PartidoPolitico.Nombre,
                        Siglas = s.PartidoPolitico.Siglas,
                        LogoUrl = s.PartidoPolitico.LogoUrl,
                        Activo = s.PartidoPolitico.Activo
                    },
                    Activo = s.Activo
                }).ToListAsync();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<CandidatoDto>> GetByPartido(int partidoId)
        {
            try
            {
                var listEntities = await _candidatoRepository.GetByPartido(partidoId);
                return listEntities.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<CandidatoDto?> GetById(int id)
        {
            try
            {
                var listEntitiesQuery = _candidatoRepository.GetAllQueryWithInclude(["PartidoPolitico"]);
                var entity = await listEntitiesQuery.FirstOrDefaultAsync(c => c.Id == id);
                if (entity == null) return null;

                return new CandidatoDto()
                {
                    Id = entity.Id,
                    Nombre = entity.Nombre,
                    Apellido = entity.Apellido,
                    FotoUrl = entity.FotoUrl,
                    PartidoPoliticoId = entity.PartidoPoliticoId,
                    PartidoPolitico = entity.PartidoPolitico == null ? null : new PartidoPoliticoDto()
                    {
                        Id = entity.PartidoPolitico.Id,
                        Nombre = entity.PartidoPolitico.Nombre,
                        Siglas = entity.PartidoPolitico.Siglas,
                        LogoUrl = entity.PartidoPolitico.LogoUrl,
                        Activo = entity.PartidoPolitico.Activo
                    },
                    Activo = entity.Activo
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> ParticipoenEleccionAsync(int id)
        {
            try
            {
                return await _candidatoRepository.ParticipoenEleccion(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static CandidatoDto MapToDto(Candidato entity)
        {
            return new CandidatoDto()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                FotoUrl = entity.FotoUrl,
                PartidoPoliticoId = entity.PartidoPoliticoId,
                Activo = entity.Activo
            };
        }
    }
}