using EVote360Pro.Core.Application.Dtos.Ciudadano;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;

namespace EVote360Pro.Core.Application.Services
{
    public class CiudadanoService : ICiudadanoService
    {
        private readonly ICiudadanoRepository _ciudadanoRepository;

        public CiudadanoService(ICiudadanoRepository ciudadanoRepository)
        {
            _ciudadanoRepository = ciudadanoRepository;
        }

        public async Task<CiudadanoDto?> AddAsync(SaveCiudadanoDto dto)
        {
            try
            {
                Ciudadano entity = new()
                {
                    Id = 0,
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    CorreoElectronico = dto.CorreoElectronico,
                    NumeroDocumento = dto.NumeroDocumento,
                    Activo = true
                };

                Ciudadano? returnEntity = await _ciudadanoRepository.AddAsync(entity);
                if (returnEntity == null) return null;

                return MapToDto(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CiudadanoDto?> UpdateAsync(SaveCiudadanoDto dto)
        {
            try
            {
                var existing = await _ciudadanoRepository.GetById(dto.Id);
                if (existing == null) return null;

                existing.Nombre = dto.Nombre;
                existing.Apellido = dto.Apellido;
                existing.CorreoElectronico = dto.CorreoElectronico;
                existing.NumeroDocumento = dto.NumeroDocumento;
                existing.Activo = dto.Activo;

                Ciudadano? returnEntity = await _ciudadanoRepository.UpdateAsync(existing.Id, existing);
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
                await _ciudadanoRepository.DeleteAsync(id);
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
                var entity = await _ciudadanoRepository.GetById(id);
                if (entity == null) return false;

                entity.Activo = !entity.Activo;
                await _ciudadanoRepository.UpdateAsync(entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<CiudadanoDto>> GetAll()
        {
            try
            {
                var listEntities = await _ciudadanoRepository.GetAllList();
                return listEntities.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<CiudadanoDto?> GetById(int id)
        {
            try
            {
                var entity = await _ciudadanoRepository.GetById(id);
                if (entity == null) return null;
                return MapToDto(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CiudadanoDto?> GetByNumeroDocumento(string numeroDocumento)
        {
            try
            {
                var entity = await _ciudadanoRepository.GetByNumeroDocumento(numeroDocumento);
                if (entity == null) return null;
                return MapToDto(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static CiudadanoDto MapToDto(Ciudadano entity)
        {
            return new CiudadanoDto()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Apellido = entity.Apellido,
                CorreoElectronico = entity.CorreoElectronico,
                NumeroDocumento = entity.NumeroDocumento,
                Activo = entity.Activo
            };
        }
    }
}