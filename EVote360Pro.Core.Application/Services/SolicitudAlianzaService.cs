using EVote360Pro.Core.Application.Dtos.PartidoPolitico;
using EVote360Pro.Core.Application.Dtos.SolicitudAlianza;
using EVote360Pro.Core.Application.Interfaces;
using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Core.Application.Services
{
    public class SolicitudAlianzaService : ISolicitudAlianzaService
    {
        private readonly ISolicitudAlianzaRepository _solicitudRepository;

        public SolicitudAlianzaService(ISolicitudAlianzaRepository solicitudRepository)
        {
            _solicitudRepository = solicitudRepository;
        }

        public async Task<SolicitudAlianzaDto?> CrearSolicitudAsync(SaveSolicitudAlianzaDto dto)
        {
            try
            {
                if (dto.PartidoSolicitanteId == dto.PartidoReceptorId) return null;

                bool existePendiente = await _solicitudRepository.ExisteSolicitudPendiente(
                    dto.PartidoSolicitanteId, dto.PartidoReceptorId);
                if (existePendiente) return null;

                bool existeAlianza = await _solicitudRepository.ExisteAlianzaVigente(
                    dto.PartidoSolicitanteId, dto.PartidoReceptorId);
                if (existeAlianza) return null;

                SolicitudAlianza entity = new()
                {
                    Id = 0,
                    PartidoSolicitanteId = dto.PartidoSolicitanteId,
                    PartidoReceptorId = dto.PartidoReceptorId,
                    FechaSolicitud = DateTime.Now,
                    Estado = EstadoSolicitudAlianza.EnEsperaDeRespuesta,
                    Activo = true
                };

                SolicitudAlianza? returnEntity = await _solicitudRepository.AddAsync(entity);
                if (returnEntity == null) return null;

                return MapToDto(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> AceptarSolicitudAsync(int id)
        {
            try
            {
                var entity = await _solicitudRepository.GetById(id);
                if (entity == null || entity.Estado != EstadoSolicitudAlianza.EnEsperaDeRespuesta)
                    return false;

                entity.Estado = EstadoSolicitudAlianza.Aceptada;
                entity.FechaRespuesta = DateTime.Now;
                await _solicitudRepository.UpdateAsync(entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RechazarSolicitudAsync(int id)
        {
            try
            {
                var entity = await _solicitudRepository.GetById(id);
                if (entity == null || entity.Estado != EstadoSolicitudAlianza.EnEsperaDeRespuesta)
                    return false;

                entity.Estado = EstadoSolicitudAlianza.Rechazada;
                entity.FechaRespuesta = DateTime.Now;
                await _solicitudRepository.UpdateAsync(entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarSolicitudAsync(int id)
        {
            try
            {
                var entity = await _solicitudRepository.GetById(id);
                if (entity == null) return false;

                if (entity.Estado == EstadoSolicitudAlianza.Aceptada) return false;

                await _solicitudRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EliminarAlianzaAsync(int id)
        {
            try
            {
                var entity = await _solicitudRepository.GetById(id);
                if (entity == null || entity.Estado != EstadoSolicitudAlianza.Aceptada) return false;

                await _solicitudRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<SolicitudAlianzaDto>> GetSolicitudesPendientesParaPartido(int partidoId)
        {
            try
            {
                var listEntities = await _solicitudRepository.GetSolicitudesPendientesParaPartido(partidoId);
                return await MapToDtoWithInclude(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<SolicitudAlianzaDto>> GetSolicitudesRealizadasPorPartido(int partidoId)
        {
            try
            {
                var listEntities = await _solicitudRepository.GetSolicitudesRealizadasPorPartido(partidoId);
                return await MapToDtoWithInclude(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<SolicitudAlianzaDto>> GetAlianzasVigentes(int partidoId)
        {
            try
            {
                var listEntities = await _solicitudRepository.GetAlianzasVigentes(partidoId);
                return await MapToDtoWithInclude(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        private static SolicitudAlianzaDto MapToDto(SolicitudAlianza entity)
        {
            return new SolicitudAlianzaDto()
            {
                Id = entity.Id,
                PartidoSolicitanteId = entity.PartidoSolicitanteId,
                PartidoReceptorId = entity.PartidoReceptorId,
                FechaSolicitud = entity.FechaSolicitud,
                FechaRespuesta = entity.FechaRespuesta,
                Estado = entity.Estado,
                Activo = entity.Activo
            };
        }

        private static Task<List<SolicitudAlianzaDto>> MapToDtoWithInclude(List<SolicitudAlianza> entities)
        {
            var result = entities.Select(s => new SolicitudAlianzaDto()
            {
                Id = s.Id,
                PartidoSolicitanteId = s.PartidoSolicitanteId,
                PartidoReceptorId = s.PartidoReceptorId,
                FechaSolicitud = s.FechaSolicitud,
                FechaRespuesta = s.FechaRespuesta,
                Estado = s.Estado,
                Activo = s.Activo,
                PartidoSolicitante = s.PartidoSolicitante == null ? null : new PartidoPoliticoDto()
                {
                    Id = s.PartidoSolicitante.Id,
                    Nombre = s.PartidoSolicitante.Nombre,
                    Siglas = s.PartidoSolicitante.Siglas,
                    LogoUrl = s.PartidoSolicitante.LogoUrl,
                    Activo = s.PartidoSolicitante.Activo
                },
                PartidoReceptor = s.PartidoReceptor == null ? null : new PartidoPoliticoDto()
                {
                    Id = s.PartidoReceptor.Id,
                    Nombre = s.PartidoReceptor.Nombre,
                    Siglas = s.PartidoReceptor.Siglas,
                    LogoUrl = s.PartidoReceptor.LogoUrl,
                    Activo = s.PartidoReceptor.Activo
                }
            }).ToList();

            return Task.FromResult(result);
        }
    }
}