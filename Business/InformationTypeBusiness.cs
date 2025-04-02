using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class InformationTypeBusiness
    {
        private readonly InformationTypeData _infoTypeData;
        private readonly ILogger<InformationTypeBusiness> _logger;

        public InformationTypeBusiness(InformationTypeData infoTypeData, ILogger<InformationTypeBusiness> logger)
        {
            _infoTypeData = infoTypeData;
            _logger = logger;
        }

        public async Task<IEnumerable<InformationTypeDto>> GetAllInformationTypesAsync()
        {
            try
            {
                var infoTypes = await _infoTypeData.GetAllAsync();
                return infoTypes.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de información");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de tipos de información", ex);
            }
        }

        public async Task<InformationTypeDto> GetInformationTypeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un tipo de información con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var infoType = await _infoTypeData.GetByIdAsync(id);
                if (infoType == null)
                {
                    _logger.LogInformation("No se encontró ningún tipo de información con ID: {Id}", id);
                    throw new EntityNotFoundException("InformationType", id);
                }

                return MapToDto(infoType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tipo de información con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el tipo de información con ID {id}", ex);
            }
        }

        private InformationTypeDto MapToDto(InformationType infoType)
        {
            throw new NotImplementedException();
        }

        public async Task<InformationTypeDto> CreateInformationTypeAsync(InformationTypeDto infoTypeDto)
        {
            try
            {
                ValidateInformationType(infoTypeDto);

                var infoType = MapToEntity(infoTypeDto);
                var createdInfoType = await _infoTypeData.CreateAsync(infoType);

                return MapToDto(createdInfoType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo tipo de información");
                throw new ExternalServiceException("Base de datos", "Error al crear el tipo de información", ex);
            }
        }

        public async Task<InformationTypeDto> UpdateInformationTypeAsync(InformationTypeDto infoTypeDto)
        {
            try
            {
                ValidateInformationType(infoTypeDto);

                var infoType = MapToEntity(infoTypeDto);
                var updated = await _infoTypeData.UpdateAsync(infoType);

                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el tipo de información");
                }

                return infoTypeDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el tipo de información");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el tipo de información", ex);
            }
        }

        public async Task DeleteInformationTypeAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un tipo de información con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var deleted = await _infoTypeData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el tipo de información");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el tipo de información con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el tipo de información con ID {id}", ex);
            }
        }

        private void ValidateInformationType(InformationTypeDto infoTypeDto)
        {
            if (infoTypeDto == null)
            {
                throw new ValidationException("El objeto tipo de información no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(infoTypeDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un tipo de información sin nombre");
                throw new ValidationException("Name", "El nombre es obligatorio");
            }
        }

        private static InformationTypeDto MapToDto(InformationType infoType) => new InformationTypeDto
        {
            Id = infoType.Id,
            Name = infoType.Name,
            Description = infoType.Description
        };

        private static InformationType MapToEntity(InformationTypeDto infoTypeDto) => new InformationType
        {
            Id = infoTypeDto.Id,
            Name = infoTypeDto.Name,
            Description = infoTypeDto.Description
        };
    }
}
