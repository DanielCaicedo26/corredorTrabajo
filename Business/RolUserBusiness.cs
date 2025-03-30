using System.ComponentModel.DataAnnotations;
using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class RoleUserBusiness
    {
        private readonly RoleUserData _roleUserData;
        private readonly ILogger<RoleUserBusiness> _logger;

        public RoleUserBusiness(RoleUserData roleUserData, ILogger<RoleUserBusiness> logger)
        {
            _roleUserData = roleUserData;
            _logger = logger;
        }

        public async Task<IEnumerable<RoleUserDto>> GetAllRoleUsersAsync()
        {
            try
            {
                var roleUsers = await _roleUserData.GetAllAsync();
                return roleUsers.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las asignaciones de roles a usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de asignaciones", ex);
            }
        }

        public async Task<RoleUserDto> GetRoleUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una asignación con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID de la asignación debe ser mayor que cero");
            }

            try
            {
                var roleUser = await _roleUserData.GetByIdAsync(id);
                if (roleUser == null)
                {
                    _logger.LogInformation("No se encontró ninguna asignación con ID: {Id}", id);
                    throw new EntityNotFoundException("RoleUser", id);
                }

                return MapToDto(roleUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la asignación con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la asignación con ID {id}", ex);
            }
        }

        public async Task<RoleUserDto> AssignRoleToUserAsync(RoleUserDto roleUserDto)
        {
            try
            {
                ValidateRoleUser(roleUserDto);

                var roleUser = MapToEntity(roleUserDto);
                var createdRoleUser = await _roleUserData.CreateAsync(roleUser);

                return MapToDto(createdRoleUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar un rol al usuario");
                throw new ExternalServiceException("Base de datos", "Error al asignar el rol", ex);
            }
        }

        public async Task RemoveRoleFromUserAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar una asignación con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID de la asignación debe ser mayor que cero");
            }

            try
            {
                var deleted = await _roleUserData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar la asignación de rol");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la asignación con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar la asignación con ID {id}", ex);
            }
        }

        private void ValidateRoleUser(RoleUserDto roleUserDto)
        {
            if (roleUserDto == null)
            {
                throw new ValidationException("El objeto de asignación de rol no puede ser nulo");
            }

            if (roleUserDto.RoleId <= 0)
            {
                _logger.LogWarning("Se intentó asignar un rol con ID inválido: {RoleId}", roleUserDto.RoleId);
                throw new ValidationException("RoleId", "El ID del rol debe ser mayor que cero");
            }

            if (roleUserDto.UserId <= 0)
            {
                _logger.LogWarning("Se intentó asignar un usuario_
