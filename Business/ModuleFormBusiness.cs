using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class ModuleFormBusiness
    {
        private readonly ModuleFormData _moduleFormData;
        private readonly ILogger<ModuleFormBusiness> _logger;

        public ModuleFormBusiness(ModuleFormData moduleFormData, ILogger<ModuleFormBusiness> logger)
        {
            _moduleFormData = moduleFormData;
            _logger = logger;
        }

        public async Task<IEnumerable<ModuleFormDto>> GetAllModuleFormsAsync()
        {
            try
            {
                var moduleForms = await _moduleFormData.GetAllAsync();
                return moduleForms.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los ModuleForm");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de ModuleForm", ex);
            }
        }

        public async Task<ModuleFormDto> GetModuleFormByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un ModuleForm con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var moduleForm = await _moduleFormData.GetByIdAsync(id);
                if (moduleForm == null)
                {
                    _logger.LogInformation("No se encontró ningún ModuleForm con ID: {Id}", id);
                    throw new EntityNotFoundException("ModuleForm", id);
                }

                return MapToDto(moduleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ModuleForm con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el ModuleForm con ID {id}", ex);
            }
        }

        public async Task<ModuleFormDto> CreateModuleFormAsync(ModuleFormDto moduleFormDto)
        {
            try
            {
                ValidateModuleForm(moduleFormDto);

                var moduleForm = MapToEntity(moduleFormDto);
                var createdModuleForm = await _moduleFormData.CreateAsync(moduleForm);

                return MapToDto(createdModuleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo ModuleForm");
                throw new ExternalServiceException("Base de datos", "Error al crear el ModuleForm", ex);
            }
        }

        public async Task<ModuleFormDto> UpdateModuleFormAsync(ModuleFormDto moduleFormDto)
        {
            try
            {
                ValidateModuleForm(moduleFormDto);

                var moduleForm = MapToEntity(moduleFormDto);
                var updated = await _moduleFormData.UpdateAsync(moduleForm);

                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el ModuleForm");
                }

                return moduleFormDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el ModuleForm");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el ModuleForm", ex);
            }
        }

        public async Task DeleteModuleFormAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un ModuleForm con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var deleted = await _moduleFormData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el ModuleForm");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el ModuleForm con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el ModuleForm con ID {id}", ex);
            }
        }

        private void ValidateModuleForm(ModuleFormDto moduleFormDto)
        {
            if (moduleFormDto == null)
            {
                throw new ValidationException("El objeto ModuleForm no puede ser nulo");
            }

            if (moduleFormDto.RolPermissionId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un ModuleForm con RolPermissionId inválido");
                throw new ValidationException("RolPermissionId", "El RolPermissionId debe ser mayor que cero");
            }

            if (moduleFormDto.FormId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un ModuleForm con FormId inválido");
                throw new ValidationException("FormId", "El FormId debe ser mayor que cero");
            }

            if (moduleFormDto.ModuleId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un ModuleForm con ModuleId inválido");
                throw new ValidationException("ModuleId", "El ModuleId debe ser mayor que cero");
            }
        }

        private static ModuleFormDto MapToDto(ModuleForm moduleForm) => new ModuleFormDto
        {
            Id = moduleForm.Id,
            RolPermissionId = moduleForm.RolPermissionId,
            FormId = moduleForm.FormId,
            ModuleId = moduleForm.ModuleId
        };

        private static ModuleForm MapToEntity(ModuleFormDto moduleFormDto) => new ModuleForm
        {
            Id = moduleFormDto.Id,
            RolPermissionId = moduleFormDto.RolPermissionId,
            FormId = moduleFormDto.FormId,
            ModuleId = moduleFormDto.ModuleId
        };
    }
}
