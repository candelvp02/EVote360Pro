using EVote360Pro.Core.Application.Dtos.PuestoElectivo;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;

namespace EVote360Pro.Core.Application.Services
{
    public class PuestoElectivoService : IPuestoElectivoService
    {
        private readonly IPuestoElectivoRepository _puestoElectivoRepository;

        public PuestoElectivoService(IPuestoElectivoRepository puestoElectivoRepository)
        {
            _puestoElectivoRepository = puestoElectivoRepository;
        }

        public async Task<PuestoElectivoDto?> AddAsync(SavePuestoElectivoDto dto)
        {
            try
            {
                PuestoElectivo entity = new()
                {
                    Id = 0,
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Activo = true
                };

                PuestoElectivo? returnEntity = await _puestoElectivoRepository.AddAsync(entity);
                if (returnEntity == null) return null;

                return MapToDto(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PuestoElectivoDto?> UpdateAsync(SavePuestoElectivoDto dto)
        {
            try
            {
                var existing = await _puestoElectivoRepository.GetById(dto.Id);
                if (existing == null) return null;

                existing.Nombre = dto.Nombre;
                existing.Descripcion = dto.Descripcion;
                existing.Activo = dto.Activo;

                PuestoElectivo? returnEntity = await _puestoElectivoRepository.UpdateAsync(existing.Id, existing);
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
                await _puestoElectivoRepository.DeleteAsync(id);
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
                var entity = await _puestoElectivoRepository.GetById(id);
                if (entity == null) return false;

                entity.Activo = !entity.Activo;
                await _puestoElectivoRepository.UpdateAsync(entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<PuestoElectivoDto>> GetAll()
        {
            try
            {
                var listEntities = await _puestoElectivoRepository.GetAllList();
                return listEntities.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<PuestoElectivoDto>> GetAllActivos()
        {
            try
            {
                var listEntities = await _puestoElectivoRepository.GetAllActivos();
                return listEntities.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<PuestoElectivoDto?> GetById(int id)
        {
            try
            {
                var entity = await _puestoElectivoRepository.GetById(id);
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
                return await _puestoElectivoRepository.ParticipoenEleccion(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static PuestoElectivoDto MapToDto(PuestoElectivo entity)
        {
            return new PuestoElectivoDto()
            {
                Id = entity.Id,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion,
                Activo = entity.Activo
            };
        }
    }
}