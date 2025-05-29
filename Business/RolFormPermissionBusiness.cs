using Data;
using Microsoft.Extensions.Logging;
using Entity.Dto;
using Utilities.Exceptions;
using Entity.Model;

namespace Business
{
    /// <summary>
    /// Lógica de negocio para la gestión de relaciones entre roles, formularios y permisos.
    /// </summary>
    public class RolFormPermissionBusiness
    {
        private readonly RolFormPermissionData _rolFormPermissionData;
        private readonly ILogger<RolFormPermissionBusiness> _logger;

        public RolFormPermissionBusiness(RolFormPermissionData rolFormPermissionData, ILogger<RolFormPermissionBusiness> logger)
        {
            _rolFormPermissionData = rolFormPermissionData;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las relaciones Rol-Form-Permiso.
        /// </summary>
        public async Task<IEnumerable<RolFormPermissionDto>> GetAllAsync()
        {
            try
            {
                var permissions = await _rolFormPermissionData.GetAllAsync();
                return MapToDTOList(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las relaciones Rol-Form-Permiso");
                throw new ExternalServiceException("Base de datos", "Error al recuperar los permisos de los roles", ex);
            }
        }

        /// <summary>
        /// Obtiene una relación Rol-Form-Permiso por su ID.
        /// </summary>
        public async Task<RolFormPermissionDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("El ID debe ser mayor que cero.");
            }

            try
            {
                var permission = await _rolFormPermissionData.GetByIdAsync(id);
                if (permission == null)
                {
                    throw new EntityNotFoundException("RolFormPermission", id);
                }

                return MapToDTO(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la relación Rol-Form-Permiso con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la relación con ID {id}", ex);
            }
        }

        /// <summary>
        /// Crea una nueva relación Rol-Form-Permiso.
        /// </summary>
        public async Task<RolFormPermissionDto> CreateAsync(RolFormPermissionDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException("Los datos de la relación son obligatorios.");
            }

            try
            {
                Validate(dto);
                var entity = MapToEntity(dto);
                var created = await _rolFormPermissionData.CreateAsync(entity);

                return MapToDTO(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva relación Rol-Form-Permiso");
                throw new ExternalServiceException("Base de datos", "Error al crear la relación", ex);
            }
        }

        /// <summary>
        /// Actualiza una relación existente Rol-Form-Permiso.
        /// </summary>
        public async Task<RolFormPermissionDto> UpdateAsync(RolFormPermissionDto dto)
        {
            if (dto == null)
            {
                throw new ValidationException("Los datos de la relación son obligatorios.");
            }

            try
            {
                Validate(dto);
                var entity = MapToEntity(dto);
                var updated = await _rolFormPermissionData.UpdateAsync(entity);
                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar la relación");
                }

                return MapToDTO(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la relación Rol-Form-Permiso");
                throw new ExternalServiceException("Base de datos", "Error al actualizar la relación", ex);
            }
        }

        /// <summary>
        /// Elimina una relación Rol-Form-Permiso por su ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("El ID debe ser mayor que cero.");
            }

            try
            {
                var deleted = await _rolFormPermissionData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar la relación");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la relación Rol-Form-Permiso con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar la relación con ID {id}", ex);
            }
        }

        /// <summary>
        /// Valida que los datos de la relación Rol-Form-Permiso sean correctos.
        /// </summary>
        private void Validate(RolFormPermissionDto dto)
        {
            if (dto.RolId <= 0)
            {
                throw new ValidationException("El RolId debe ser mayor que cero.");
            }

            if (dto.FormId <= 0)
            {
                throw new ValidationException("El FormId debe ser mayor que cero.");
            }

            if (dto.PermissionId <= 0)
            {
                throw new ValidationException("El PermissionId debe ser mayor que cero.");
            }
        }

        // Método para mapear de RolFormPermission a RolFormPermissionDto
        private static RolFormPermissionDto MapToDTO(RolFormPermission entity)
        {
            return new RolFormPermissionDto
            {
                Id = entity.Id,
                RolId = entity.RolId,
                FormId = entity.FormId,
                PermissionId = entity.PermissionId
            };
        }

        // Método para mapear de RolFormPermissionDto a RolFormPermission
        private static RolFormPermission MapToEntity(RolFormPermissionDto dto)
        {
            return new RolFormPermission
            {
                Id = dto.Id,
                RolId = dto.RolId,
                FormId = dto.FormId,
                PermissionId = dto.PermissionId
            };
        }

        // Método para mapear una lista de RolFormPermission a una lista de RolFormPermissionDto
        private IEnumerable<RolFormPermissionDto> MapToDTOList(IEnumerable<RolFormPermission> permissions)
        {
            var permissionsDto = new List<RolFormPermissionDto>();
            foreach (var permission in permissions)
            {
                permissionsDto.Add(MapToDTO(permission));
            }
            return permissionsDto;
        }
    }
}







