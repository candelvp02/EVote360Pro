using EVote360Pro.Core.Application.Dtos.DirigentePolitico;
using EVote360Pro.Core.Application.Dtos.PartidoPolitico;
using EVote360Pro.Core.Application.Dtos.Usuario;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Core.Application.Services
{
    public class DirigentePoliticoService : IDirigentePoliticoService
    {
        private readonly IDirigentePoliticoRepository _dirigenteRepository;

        public DirigentePoliticoService(IDirigentePoliticoRepository dirigenteRepository)
        {
            _dirigenteRepository = dirigenteRepository;
        }

        public async Task<DirigentePoliticoDto?> AddAsync(SaveDirigentePoliticoDto dto)
        {
            try
            {
                DirigentePolitico entity = new()
                {
                    Id = 0,
                    UsuarioId = dto.UsuarioId,
                    PartidoPoliticoId = dto.PartidoPoliticoId,
                    Activo = true
                };

                DirigentePolitico? returnEntity = await _dirigenteRepository.AddAsync(entity);
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
                await _dirigenteRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<DirigentePoliticoDto>> GetAll()
        {
            try
            {
                var listEntities = await _dirigenteRepository.GetAllList();
                return listEntities.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<DirigentePoliticoDto>> GetAllWithInclude()
        {
            try
            {
                var listEntitiesQuery = _dirigenteRepository.GetAllQueryWithInclude(["Usuario", "PartidoPolitico"]);
                return await listEntitiesQuery.Select(s => new DirigentePoliticoDto()
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    PartidoPoliticoId = s.PartidoPoliticoId,
                    Activo = s.Activo,
                    Usuario = s.Usuario == null ? null : new UsuarioDto()
                    {
                        Id = s.Usuario.Id,
                        Nombre = s.Usuario.Nombre,
                        Apellido = s.Usuario.Apellido,
                        NombreUsuario = s.Usuario.NombreUsuario,
                        CorreoElectronico = s.Usuario.CorreoElectronico,
                        Rol = s.Usuario.Rol,
                        Activo = s.Usuario.Activo
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

        public async Task<DirigentePoliticoDto?> GetById(int id)
        {
            try
            {
                var entity = await _dirigenteRepository.GetById(id);
                if (entity == null) return null;
                return MapToDto(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<DirigentePoliticoDto?> GetByUsuarioId(int usuarioId)
        {
            try
            {
                var listEntitiesQuery = _dirigenteRepository.GetAllQueryWithInclude(["Usuario", "PartidoPolitico"]);
                var entity = await listEntitiesQuery.FirstOrDefaultAsync(d => d.UsuarioId == usuarioId);
                if (entity == null) return null;

                return new DirigentePoliticoDto()
                {
                    Id = entity.Id,
                    UsuarioId = entity.UsuarioId,
                    PartidoPoliticoId = entity.PartidoPoliticoId,
                    Activo = entity.Activo,
                    Usuario = entity.Usuario == null ? null : new UsuarioDto()
                    {
                        Id = entity.Usuario.Id,
                        Nombre = entity.Usuario.Nombre,
                        Apellido = entity.Usuario.Apellido,
                        NombreUsuario = entity.Usuario.NombreUsuario,
                        CorreoElectronico = entity.Usuario.CorreoElectronico,
                        Rol = entity.Usuario.Rol,
                        Activo = entity.Usuario.Activo
                    },
                    PartidoPolitico = entity.PartidoPolitico == null ? null : new PartidoPoliticoDto()
                    {
                        Id = entity.PartidoPolitico.Id,
                        Nombre = entity.PartidoPolitico.Nombre,
                        Siglas = entity.PartidoPolitico.Siglas,
                        LogoUrl = entity.PartidoPolitico.LogoUrl,
                        Activo = entity.PartidoPolitico.Activo
                    }
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static DirigentePoliticoDto MapToDto(DirigentePolitico entity)
        {
            return new DirigentePoliticoDto()
            {
                Id = entity.Id,
                UsuarioId = entity.UsuarioId,
                PartidoPoliticoId = entity.PartidoPoliticoId,
                Activo = entity.Activo
            };
        }
    }
}