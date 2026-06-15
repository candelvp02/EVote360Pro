using EVote360Pro.Core.Application.Dtos.PartidoPolitico;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;

namespace EVote360Pro.Core.Application.Services
{
    public class PartidoPoliticoService : IPartidoPoliticoService
    {
        private readonly IPartidoPoliticoRepository _partidoPoliticoRepository;

        public PartidoPoliticoService(IPartidoPoliticoRepository partidoPoliticoRepository)
        {
            _partidoPoliticoRepository = partidoPoliticoRepository;
        }

        public async Task<PartidoPoliticoDto?> AddAsync(SavePartidoPoliticoDto dto)
        {
            try
            {
                PartidoPolitico entity = new()
                {
                    Id = 0,
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Siglas = dto.Siglas,
                    LogoUrl = dto.LogoUrl,
                    Activo = true
                };

                PartidoPolitico? returnEntity = await _partidoPoliticoRepository.AddAsync(entity);
                if (returnEntity == null) return null;

                return MapToDto(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PartidoPoliticoDto?> UpdateAsync(SavePartidoPoliticoDto dto)
        {
            try
            {
                var existing = await _partidoPoliticoRepository.GetById(dto.Id);
                if (existing == null) return null;

                existing.Nombre = dto.Nombre;
                existing.Descripcion = dto.Descripcion;
                existing.Siglas = dto.Siglas;
                existing.LogoUrl = dto.LogoUrl;
                existing.Activo = dto.Activo;

                PartidoPolitico? returnEntity = await _partidoPoliticoRepository.UpdateAsync(existing.Id, existing);
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
                await _partidoPoliticoRepository.DeleteAsync(id);
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
                var entity = await _partidoPoliticoRepository.GetById(id);
                if (entity == null) return false;

                entity.Activo = !entity.Activo;
                await _partidoPoliticoRepository.UpdateAsync(entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<PartidoPoliticoDto>> GetAll()
        {
            try
            {
                var listEntities = await _partidoPoliticoRepository.GetAllList();
                return listEntities.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<PartidoPoliticoDto>> GetAllActivos()
        {
            try
            {
                var listEntities = await _partidoPoliticoRepository.GetAllActivos();
                return listEntities.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<PartidoPoliticoDto?> GetById(int id)
        {
            try
            {
                var entity = await _partidoPoliticoRepository.GetById(id);
                if (entity == null) return null;
                return MapToDto(entity);
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
                return await _partidoPoliticoRepository.ParticipoenEleccion(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static PartidoPoliticoDto MapToDto(PartidoPolitico entity)
        {
            return new PartidoPoliticoDto()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion,
                Siglas = entity.Siglas,
                LogoUrl = entity.LogoUrl,
                Activo = entity.Activo
            };
        }
    }
}