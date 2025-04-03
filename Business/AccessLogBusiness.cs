using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Lógica de negocio para la gestión de registros de acceso.
    /// </summary>
    public class AccessLogBusiness
    {
        private readonly AccessLogData _accessLogData;
        private readonly ILogger<AccessLogBusiness> _logger;

        public AccessLogBusiness(AccessLogData accessLogData, ILogger<AccessLogBusiness> logger)
        {
            _accessLogData = accessLogData;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los registros de acceso.
        /// </summary>
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

        /// <summary>
        /// Obtiene un registro de acceso por su ID.
        /// </summary>
        public async Task<AccessLogDto> GetLogByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("id", "El ID del registro debe ser mayor que cero");
            }

            try
            {
                var log = await _accessLogData.GetByIdAsync(id);
                if (log == null)
                {
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

        /// <summary>
        /// Crea un nuevo registro de acceso.
        /// </summary>
        public async Task<AccessLogDto> CreateLogAsync(AccessLogDto logDto)
        {
            if (logDto == null)
            {
                throw new ValidationException("El objeto de registro de acceso no puede ser nulo");
            }

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

        /// <summary>
        /// Valida que los datos del registro de acceso sean correctos.
        /// </summary>
        private void ValidateLog(AccessLogDto logDto)
        {
            if (string.IsNullOrWhiteSpace(logDto.Action))
            {
                throw new ValidationException("Action", "La acción del registro es obligatoria");
            }

            if (string.IsNullOrWhiteSpace(logDto.Status))
            {
                throw new ValidationException("Status", "El estado del registro es obligatorio");
            }
        }

        /// <summary>
        /// Convierte una entidad en un DTO.
        /// </summary>
        private static AccessLogDto MapToDto(AccessLog entity)
        {
            return new AccessLogDto
            {
                Id = entity.Id,
                Action = entity.Action,
                Timestamp = entity.Timestamp,
                Status = entity.Status,
                Details = entity.Details
            };
        }

        /// <summary>
        /// Convierte un DTO en una entidad.
        /// </summary>
        private static AccessLog MapToEntity(AccessLogDto dto)
        {
            return new AccessLog
            {
                Id = dto.Id,
                Action = dto.Action,
                Timestamp = dto.Timestamp,
                Status = dto.Status,
                Details = dto.Details
            };
        }
    }
}
