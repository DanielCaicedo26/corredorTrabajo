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
                return MapToDTOList(modules);
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

                return MapToDTO(module);
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

                return MapToDTO(createdModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo módulo: {ModuleName}", moduleDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el módulo", ex);
            }
        }

        public async Task<ModuleDto> UpdateModuleAsync(int id, ModuleDto moduleDto)
        {
            ValidateModule(moduleDto);

            var existingModule = await _moduleData.GetByIdAsync(id);
            if (existingModule == null)
            {
                _logger.LogWarning("Intento de actualizar un módulo inexistente con ID: {Id}", id);
                throw new EntityNotFoundException("Module", id);
            }

            existingModule.Name = moduleDto.Name;
            existingModule.Description = moduleDto.Description;
            existingModule.Status = moduleDto.Status;

            await _moduleData.UpdateAsync(existingModule);

            return MapToDTO(existingModule);
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
                var module = await _moduleData.GetByIdAsync(id);
                if (module == null)
                {
                    _logger.LogWarning("Intento de eliminar un módulo inexistente con ID: {Id}", id);
                    throw new EntityNotFoundException("Module", id);
                }

                await _moduleData.DeleteAsync(id);
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

        // Método para mapear de Module a ModuleDto
        private ModuleDto MapToDTO(Module module)
        {
            return new ModuleDto
            {
                Id = module.Id,
                Name = module.Name,
                Description = module.Description,
                Status = module.Status
            };
        }

        // Método para mapear de ModuleDto a Module
        private Module MapToEntity(ModuleDto moduleDto)
        {
            return new Module
            {
                Id = moduleDto.Id,
                Name = moduleDto.Name,
                Description = moduleDto.Description,
                Status = moduleDto.Status
            };
        }

        // Método para mapear una lista de Module a una lista de ModuleDto
        private IEnumerable<ModuleDto> MapToDTOList(IEnumerable<Module> modules)
        {
            var modulesDto = new List<ModuleDto>();
            foreach (var module in modules)
            {
                modulesDto.Add(MapToDTO(module));
            }
            return modulesDto;
        }
    }
}



