using Data;
using System.ComponentModel.DataAnnotations;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Abp.Domain.Entities;

namespace Business
{
    public class AccessLogBusiness
    {
        private readonly AccessLogData _accessLogData; // Cambiado de AccessLogDto a AccessLogData
        private readonly ILogger<AccessLogBusiness> _logger;

        public AccessLogBusiness(AccessLogData accessLogData, ILogger<AccessLogBusiness> logger)
        {
            _accessLogData = accessLogData;
            _logger = logger;
        }

        public async Task<IEnumerable<AccessLogDto>> GetAllLogsAsync()
        {
            try
            {
                var logs = await _accessLogData.GetAllAsync();
                return logs.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de acceso");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de registros de acceso", ex);
            }
        }

        public async Task<AccessLogDto> GetLogByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un registro de acceso con ID inválido: {LogId}", id);
                throw new ValidationException("id", "El ID del registro debe ser mayor que cero");
            }

            try
            {
                var log = await _accessLogData.GetByIdAsync(id);
                if (log == null)
                {
                    _logger.LogInformation("No se encontró ningún registro con ID: {LogId}", id);
                    throw new EntityNotFoundException("AccessLog", id);
                }

                return MapToDto(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de acceso con ID: {LogId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el registro con ID {id}", ex);
            }
        }

        public async Task<AccessLogDto> CreateLogAsync(AccessLogDto logDto)
        {
            try
            {
                ValidateLog(logDto);

                var log = MapToEntity(logDto);
                var createdLog = await _accessLogData.CreateAsync(log);

                return MapToDto(createdLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo registro de acceso");
                throw new ExternalServiceException("Base de datos", "Error al crear el registro de acceso", ex);
            }
        }

        public async Task DeleteLogAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un registro de acceso con ID inválido: {LogId}", id);
                throw new ValidationException("id", "El ID del registro debe ser mayor que cero");
            }

            try
            {
                var deleted = await _accessLogData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el registro de acceso");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de acceso con ID: {LogId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el registro con ID {id}", ex);
            }
        }

        private void ValidateLog(AccessLogDto logDto)
        {
            if (logDto == null)
            {
                throw new ValidationException("El objeto de registro de acceso no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(logDto.Action))
            {
                _logger.LogWarning("Se intentó crear un registro de acceso con una acción vacía");
                throw new ValidationException("Action", "La acción del registro es obligatoria");
            }

            if (string.IsNullOrWhiteSpace(logDto.Status))
            {
                _logger.LogWarning("Se intentó crear un registro de acceso con un estado vacío");
                throw new ValidationException("Status", "El estado del registro es obligatorio");
            }
        }

        private static AccessLogDto MapToDto(AccessLog log)
        {
            return new AccessLogDto
            {
                Id = log.Id,
                Action = log.Action,
                Timestamp = log.Timestamp,
                Status = log.Status,
                Details = log.Details
            };
        }

        private static AccessLog MapToEntity(AccessLogDto logDto)
        {
            return new AccessLog
            {
                Id = logDto.Id,
                Action = logDto.Action,
                Timestamp = logDto.Timestamp,
                Status = logDto.Status,
                Details = logDto.Details
            };
        }
    }
}
