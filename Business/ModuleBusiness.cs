using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class ModuleBusiness
    {
        private readonly ModuleData _moduleData;
        private readonly ILogger<ModuleBusiness> _logger;

        public ModuleBusiness(ModuleData moduleData, ILogger<ModuleBusiness> logger)
        {
            _moduleData = moduleData;
            _logger = logger;
        }

        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        {
            try
            {
                var modules = await _moduleData.GetAllAsync();
                return modules.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los módulos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de módulos", ex);
            }
        }

        public async Task<ModuleDto> GetModuleByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un módulo con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var module = await _moduleData.GetByIdAsync(id);
                if (module == null)
                {
                    _logger.LogInformation("No se encontró ningún módulo con ID: {Id}", id);
                    throw new EntityNotFoundException("Module", id);
                }

                return MapToDto(module);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el módulo con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el módulo con ID {id}", ex);
            }
        }

        public async Task<ModuleDto> CreateModuleAsync(ModuleDto moduleDto)
        {
            try
            {
                ValidateModule(moduleDto);

                var module = MapToEntity(moduleDto);
                var createdModule = await _moduleData.CreateAsync(module);

                return MapToDto(createdModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo módulo");
                throw new ExternalServiceException("Base de datos", "Error al crear el módulo", ex);
            }
        }

        public async Task<ModuleDto> UpdateModuleAsync(ModuleDto moduleDto)
        {
            try
            {
                ValidateModule(moduleDto);

                var module = MapToEntity(moduleDto);
                var updated = await _moduleData.UpdateAsync(module);

                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el módulo");
                }

                return moduleDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el módulo");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el módulo", ex);
            }
        }

        public async Task DeleteModuleAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un módulo con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var deleted = await _moduleData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el módulo");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el módulo con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el módulo con ID {id}", ex);
            }
        }

        private void ValidateModule(ModuleDto moduleDto)
        {
            if (moduleDto == null)
            {
                throw new ValidationException("El objeto módulo no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(moduleDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un módulo sin nombre");
                throw new ValidationException("Name", "El nombre es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(moduleDto.Status))
            {
                _logger.LogWarning("Se intentó crear/actualizar un módulo sin estado");
                throw new ValidationException("Status", "El estado es obligatorio");
            }
        }

        private static ModuleDto MapToDto(Module module) => new ModuleDto
        {
            Id = module.Id,
            Name = module.Name,
            Description = module.Description,
            Status = module.Status
        };

        private static Module MapToEntity(ModuleDto moduleDto) => new Module
        {
            Id = moduleDto.Id,
            Name = moduleDto.Name,
            Description = moduleDto.Description,
            Status = moduleDto.Status
        };
    }
}
