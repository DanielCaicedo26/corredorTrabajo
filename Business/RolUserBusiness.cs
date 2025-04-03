
using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class RolUserBusiness
    {
        private readonly RolUserData _rolUserData;
        private readonly ILogger<RolUserBusiness> _logger;

        public RolUserBusiness(RolUserData rolUserData, ILogger<RolUserBusiness> logger)
        {
            _rolUserData = rolUserData;
            _logger = logger;
        }

        public async Task<IEnumerable<RolUserDto>> GetAllRolUsersAsync()
        {
            try
            {
                var rolesUsers = await _rolUserData.GetAllAsync();
                return rolesUsers.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles de usuario");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles de usuario", ex);
            }
        }

        public async Task<RolUserDto> GetRolUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un RolUser con ID inválido: {RolUserId}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var rolUser = await _rolUserData.GetByIdAsync(id);
                if (rolUser == null)
                {
                    _logger.LogInformation("No se encontró ningún RolUser con ID: {RolUserId}", id);
                    throw new EntityNotFoundException("RolUser", id);
                }

                return MapToDto(rolUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RolUser con ID: {RolUserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el RolUser con ID {id}", ex);
            }
        }

        public async Task<RolUserDto> CreateRolUserAsync(RolUserDto rolUserDto)
        {
            try
            {
                ValidateRolUser(rolUserDto);

                var rolUser = new RolUser
                {
                    RolId = rolUserDto.RolId,
                    UserId = rolUserDto.UserId
                };

                var createdRolUser = await _rolUserData.CreateAsync(rolUser);
                return MapToDto(createdRolUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo RolUser");
                throw new ExternalServiceException("Base de datos", "Error al crear el RolUser", ex);
            }
        }

        public async Task<RolUserDto> UpdateRolUserAsync(RolUserDto rolUserDto)
        {
            try
            {
                ValidateRolUser(rolUserDto);

                var rolUser = new RolUser
                {
                    Id = rolUserDto.Id,
                    RolId = rolUserDto.RolId,
                    UserId = rolUserDto.UserId
                };

                var updated = await _rolUserData.UpdateAsync(rolUser);
                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el RolUser");
                }

                return rolUserDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el RolUser con ID: {RolUserId}", rolUserDto?.Id);
                throw new ExternalServiceException("Base de datos", "Error al actualizar el RolUser", ex);
            }
        }

        public async Task DeleteRolUserAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un RolUser con ID inválido: {RolUserId}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var deleted = await _rolUserData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el RolUser");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el RolUser con ID: {RolUserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el RolUser con ID {id}", ex);
            }
        }

        private void ValidateRolUser(RolUserDto rolUserDto)
        {
            if (rolUserDto == null)
            {
                throw new ValidationException("El objeto RolUser no puede ser nulo");
            }

            if (rolUserDto.RolId <= 0)
            {
                _logger.LogWarning("Se intentó asignar un RolUser con RolId inválido");
                throw new ValidationException("RolId", "El RolId debe ser mayor que cero");
            }

            if (rolUserDto.UserId <= 0)
            {
                _logger.LogWarning("Se intentó asignar un RolUser con UserId inválido");
                throw new ValidationException("UserId", "El UserId debe ser mayor que cero");
            }
        }

        private static RolUserDto MapToDto(RolUser rolUser)
        {
            return new RolUserDto
            {
                Id = rolUser.Id,
                RolId = rolUser.RolId,
                UserId = rolUser.UserId
            };
        }

        private static RolUser MapToEntity(RolUserDto rolUserDto)
        {
            return new RolUser
            {
                Id = rolUserDto.Id,
                RolId = rolUserDto.RolId,
                UserId = rolUserDto.UserId
            };
        }
    }
}
