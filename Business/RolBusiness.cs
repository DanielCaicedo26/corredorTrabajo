using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class RolBusiness
    {
        private readonly RolData _rolData;
        private readonly ILogger<RolBusiness> _logger;

        public RolBusiness(RolData rolData, ILogger<RolBusiness> logger)
        {
            _rolData = rolData;
            _logger = logger;
        }

        public async Task<IEnumerable<RolDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _rolData.GetAllAsync();
                var rolesDTO = roles.Select(rol => new RolDto
                {
                    Id = rol.Id,
                    Name = rol.Name,
                    Description = rol.Description, // Asegúrate de mapear todos los campos necesarios
                    Active = rol.Active
                }).ToList();

                return rolesDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        public async Task<RolDto> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var rol = await _rolData.GetByIdAsync(id);
                if (rol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return new RolDto
                {
                    Id = rol.Id,
                    Name = rol.Name,
                    Description = rol.Description, // Asegúrate de mapear todos los campos necesarios
                    Active = rol.Active
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        public async Task<RolDto> CreateRolAsync(RolDto rolDto)
        {
            try
            {
                ValidateRol(rolDto);

                var rol = new Rol
                {
                    Name = rolDto.Name,
                    Description = rolDto.Description, // Asegúrate de mapear todos los campos necesarios
                    Active = rolDto.Active
                };

                var rolCreado = await _rolData.CreateAsync(rol);

                return new RolDto
                {
                    Id = rolCreado.Id,
                    Name = rolCreado.Name,
                    Description = rolCreado.Description, // Asegúrate de mapear todos los campos necesarios
                    Active = rolCreado.Active
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", rolDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        public async Task<RolDto> UpdateRolAsync(RolDto rolDto)
        {
            try
            {
                ValidateRol(rolDto);

                var rol = new Rol
                {
                    Id = rolDto.Id,
                    Name = rolDto.Name,
                    Description = rolDto.Description, // Asegúrate de mapear todos los campos necesarios
                    Active = rolDto.Active
                };

                var actualizado = await _rolData.UpdateAsync(rol);
                if (!actualizado)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el rol");
                }

                return rolDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol: {RolNombre}", rolDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el rol", ex);
            }
        }

        public async Task DeleteRolAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var eliminado = await _rolData.DeleteAsync(id);
                if (!eliminado)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el rol");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el rol con ID {id}", ex);
            }
        }

        private void ValidateRol(RolDto rolDto)
        {
            if (rolDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(rolDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }
    }
}
