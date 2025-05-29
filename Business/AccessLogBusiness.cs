using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los registros de acceso.
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
        /// Obtiene todos los registros de acceso en el sistema
        /// </summary>
        /// <returns>Lista de registros de acceso como DTOs</returns>
        public async Task<IEnumerable<AccessLogDto>> GetAllAccessLogsAsync()
        {
            try
            {
                var accessLogs = await _accessLogData.GetAllAsync();
                return MapToDTOList(accessLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de acceso");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de registros de acceso", ex);
            }
        }

        /// <summary>
        /// Obtiene un registro de acceso por su ID
        /// </summary>
        /// <param name="id">ID del registro de acceso</param>
        /// <returns>Registro de acceso encontrado</returns>
        /// <exception cref="ValidationException">Si el ID es inválido</exception>
        /// <exception cref="EntityNotFoundException">Si el registro de acceso no existe</exception>
        public async Task<AccessLogDto> GetAccessLogByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un registro de acceso con ID inválido: {AccessLogId}", id);
                throw new ValidationException("id", "El ID del registro de acceso debe ser mayor que cero");
            }

            try
            {
                var accessLog = await _accessLogData.GetByIdAsync(id);
                if (accessLog == null)
                {
                    _logger.LogInformation("No se encontró ningún registro de acceso con ID: {AccessLogId}", id);
                    throw new EntityNotFoundException("Registro de acceso", id);
                }

                return MapToDTO(accessLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de acceso con ID: {AccessLogId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el registro de acceso con ID {id}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo registro de acceso en el sistema
        /// </summary>
        /// <param name="accessLogDto">Datos del registro de acceso a crear</param>
        /// <returns>Registro de acceso creado</returns>
        /// <exception cref="ValidationException">Si los datos del registro de acceso son inválidos</exception>
        public async Task<AccessLogDto> CreateAccessLogAsync(AccessLogDto accessLogDto)
        {
            ValidateAccessLog(accessLogDto);

            try
            {
                var accessLog = MapToEntity(accessLogDto);
                var createdAccessLog = await _accessLogData.CreateAsync(accessLog);
                return MapToDTO(createdAccessLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo registro de acceso: {Action}", accessLogDto?.Action ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el registro de acceso", ex);
            }
        }

        /// <summary>
        /// Elimina un registro de acceso por su ID
        /// </summary>
        /// <param name="id">ID del registro de acceso a eliminar</param>
        /// <exception cref="ValidationException">Si el ID es inválido</exception>
        /// <exception cref="EntityNotFoundException">Si el registro de acceso no existe</exception>
        public async Task DeleteAccessLogAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un registro de acceso con ID inválido: {AccessLogId}", id);
                throw new ValidationException("id", "El ID del registro de acceso debe ser mayor que cero");
            }

            try
            {
                var accessLog = await _accessLogData.GetByIdAsync(id);
                if (accessLog == null)
                {
                    _logger.LogInformation("No se encontró ningún registro de acceso para eliminar con ID: {AccessLogId}", id);
                    throw new EntityNotFoundException("Registro de acceso", id);
                }

                var deleted = await _accessLogData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el registro de acceso");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de acceso con ID: {AccessLogId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el registro de acceso con ID {id}", ex);
            }
        }

        /// <summary>
        /// Valida los datos del registro de acceso antes de su creación o actualización
        /// </summary>
        /// <param name="accessLogDto">Objeto AccessLogDto a validar</param>
        /// <exception cref="ValidationException">Si los datos no son válidos</exception>
        private void ValidateAccessLog(AccessLogDto accessLogDto)
        {
            if (accessLogDto == null)
            {
                throw new ValidationException("El objeto registro de acceso no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(accessLogDto.Action))
            {
                _logger.LogWarning("Intento de crear un registro de acceso con Action vacío");
                throw new ValidationException("Action", "La acción del registro de acceso es obligatoria");
            }
        }

        // Método para mapear de AccessLog a AccessLogDto
        private AccessLogDto MapToDTO(AccessLog accessLog)
        {
            return new AccessLogDto
            {
                Id = accessLog.Id,
                Action = accessLog.Action,
                Timestamp = accessLog.Timestamp,
                Status = accessLog.Status,
                Details = accessLog.Details
            };
        }

        // Método para mapear de AccessLogDto a AccessLog
        private AccessLog MapToEntity(AccessLogDto accessLogDto)
        {
            return new AccessLog
            {
                Id = accessLogDto.Id,
                Action = accessLogDto.Action,
                Timestamp = accessLogDto.Timestamp,
                Status = accessLogDto.Status,
                Details = accessLogDto.Details
            };
        }

        // Método para mapear una lista de AccessLog a una lista de AccessLogDto
        private IEnumerable<AccessLogDto> MapToDTOList(IEnumerable<AccessLog> accessLogs)
        {
            var accessLogsDto = new List<AccessLogDto>();
            foreach (var accessLog in accessLogs)
            {
                accessLogsDto.Add(MapToDTO(accessLog));
            }
            return accessLogsDto;
        }
    }
}



