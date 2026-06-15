using EVote360Pro.Core.Application.Dtos.Usuario;
using EVote360Pro.Core.Application.Helpers;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Core.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioDto?> AddAsync(SaveUsuarioDto dto)
        {
            try
            {
                Usuario entity = new()
                {
                    Id = 0,
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    CorreoElectronico = dto.CorreoElectronico,
                    NombreUsuario = dto.NombreUsuario,
                    Contrasena = PasswordEncryption.Encrypt(dto.Contrasena),
                    Rol = dto.Rol,
                    Activo = true
                };

                Usuario? returnEntity = await _usuarioRepository.AddAsync(entity);

                if (returnEntity == null) return null;

                return new UsuarioDto()
                {
                    Id = returnEntity.Id,
                    Nombre = returnEntity.Nombre,
                    Apellido = returnEntity.Apellido,
                    CorreoElectronico = returnEntity.CorreoElectronico,
                    NombreUsuario = returnEntity.NombreUsuario,
                    Contrasena = returnEntity.Contrasena,
                    Rol = returnEntity.Rol,
                    Activo = returnEntity.Activo
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UsuarioDto?> UpdateAsync(SaveUsuarioDto dto)
        {
            try
            {
                var existing = await _usuarioRepository.GetById(dto.Id);
                if (existing == null) return null;

                existing.Nombre = dto.Nombre;
                existing.Apellido = dto.Apellido;
                existing.CorreoElectronico = dto.CorreoElectronico;
                existing.NombreUsuario = dto.NombreUsuario;
                existing.Rol = dto.Rol;
                existing.Activo = dto.Activo;

                if (!string.IsNullOrWhiteSpace(dto.Contrasena))
                {
                    existing.Contrasena = PasswordEncryption.Encrypt(dto.Contrasena);
                }

                Usuario? returnEntity = await _usuarioRepository.UpdateAsync(existing.Id, existing);

                if (returnEntity == null) return null;

                return new UsuarioDto()
                {
                    Id = returnEntity.Id,
                    Nombre = returnEntity.Nombre,
                    Apellido = returnEntity.Apellido,
                    CorreoElectronico = returnEntity.CorreoElectronico,
                    NombreUsuario = returnEntity.NombreUsuario,
                    Contrasena = returnEntity.Contrasena,
                    Rol = returnEntity.Rol,
                    Activo = returnEntity.Activo
                };
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
                await _usuarioRepository.DeleteAsync(id);
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
                var entity = await _usuarioRepository.GetById(id);
                if (entity == null) return false;

                if (entity.Activo && entity.Rol == RolUsuario.Administrador)
                {
                    int totalAdminsActivos = await _usuarioRepository.ContarAdministradoresActivos();
                    if (totalAdminsActivos <= 1)
                        return false;
                }

                entity.Activo = !entity.Activo;
                await _usuarioRepository.UpdateAsync(entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<UsuarioDto>> GetAll()
        {
            try
            {
                var listEntities = await _usuarioRepository.GetAllList();
                return listEntities.Select(s => new UsuarioDto()
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Apellido = s.Apellido,
                    CorreoElectronico = s.CorreoElectronico,
                    NombreUsuario = s.NombreUsuario,
                    Contrasena = s.Contrasena,
                    Rol = s.Rol,
                    Activo = s.Activo
                }).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<UsuarioDto>> GetAllWithInclude()
        {
            try
            {
                var listEntities = await _usuarioRepository.GetAllList();
                return listEntities.Select(s => new UsuarioDto()
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Apellido = s.Apellido,
                    CorreoElectronico = s.CorreoElectronico,
                    NombreUsuario = s.NombreUsuario,
                    Contrasena = s.Contrasena,
                    Rol = s.Rol,
                    Activo = s.Activo
                }).ToList();
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<UsuarioDto?> GetById(int id)
        {
            try
            {
                var entity = await _usuarioRepository.GetById(id);
                if (entity == null) return null;

                return new UsuarioDto()
                {
                    Id = entity.Id,
                    Nombre = entity.Nombre,
                    Apellido = entity.Apellido,
                    CorreoElectronico = entity.CorreoElectronico,
                    NombreUsuario = entity.NombreUsuario,
                    Contrasena = entity.Contrasena,
                    Rol = entity.Rol,
                    Activo = entity.Activo
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UsuarioDto?> LoginAsync(string nombreUsuario, string contrasena)
        {
            try
            {
                string contrasenaEncriptada = PasswordEncryption.Encrypt(contrasena);
                var entity = await _usuarioRepository.GetByNombreUsuario(nombreUsuario);

                if (entity == null || entity.Contrasena != contrasenaEncriptada || !entity.Activo)
                    return null;

                return new UsuarioDto()
                {
                    Id = entity.Id,
                    Nombre = entity.Nombre,
                    Apellido = entity.Apellido,
                    CorreoElectronico = entity.CorreoElectronico,
                    NombreUsuario = entity.NombreUsuario,
                    Contrasena = entity.Contrasena,
                    Rol = entity.Rol,
                    Activo = entity.Activo
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}