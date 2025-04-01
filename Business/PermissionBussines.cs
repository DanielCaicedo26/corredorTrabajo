using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class PermissionBusiness
    {
        private readonly PermissionData _permissionData;
        private readonly ILogger<PermissionBusiness> _logger;

        public PermissionBusiness(PermissionData permissionData, ILogger<PermissionBusiness> logger)
        {
            _permissionData = permissionData;
            _logger = logger;
        }

        public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
        {
            try
            {
                var permissions = await _permissionData.GetAllAsync();
                return permissions.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos", ex);
            }
        }

        public async Task<PermissionDto> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID del permiso debe ser mayor que cero");
            }

            try
            {
                var permission = await _permissionData.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso con ID: {Id}", id);
                    throw new EntityNotFoundException("Permission", id);
                }

                return MapToDto(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
            }
        }

        public async Task<PermissionDto> CreatePermissionAsync(PermissionDto permissionDto)
        {
            try
            {
                ValidatePermission(permissionDto);

                var permission = MapToEntity(permissionDto);
                var createdPermission = await _permissionData.CreateAsync(permission);

                return MapToDto(createdPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo permiso");
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
            }
        }

        public async Task<PermissionDto> UpdatePermissionAsync(PermissionDto permissionDto)
        {
            try
            {
                ValidatePermission(permissionDto);

                var permission = MapToEntity(permissionDto);
                var updated = await _permissionData.UpdateAsync(permission);

                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el permiso");
                }

                return permissionDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el permiso", ex);
            }
        }

        public async Task DeletePermissionAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un permiso con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID del permiso debe ser mayor que cero");
            }

            try
            {
                var deleted = await _permissionData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el permiso");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el permiso con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el permiso con ID {id}", ex);
            }
        }

        private void ValidatePermission(PermissionDto permissionDto)
        {
            if (permissionDto == null)
            {
                throw new ValidationException("El objeto Permission no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(permissionDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un permiso con Name vacío");
                throw new ValidationException("Name", "El Name del permiso es obligatorio");
            }
        }

        private static PermissionDto MapToDto(Permission permission) => new PermissionDto
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description
        };

        private static Permission MapToEntity(PermissionDto permissionDto) => new Permission
        {
            Id = permissionDto.Id,
            Name = permissionDto.Name,
            Description = permissionDto.Description
        };
    }
}
