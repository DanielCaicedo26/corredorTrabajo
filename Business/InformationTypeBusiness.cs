using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los tipos de información.
    /// </summary>
    public class InformationTypeBusiness
    {
        private readonly InformationTypeData _infoTypeData;
        private readonly ILogger<InformationTypeBusiness> _logger;

        public InformationTypeBusiness(InformationTypeData infoTypeData, ILogger<InformationTypeBusiness> logger)
        {
            _infoTypeData = infoTypeData;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los tipos de información en el sistema.
        /// </summary>
        public async Task<IEnumerable<InformationTypeDto>> GetAllInformationTypesAsync()
        {
            try
            {
                var infoTypes = await _infoTypeData.GetAllAsync();
                return infoTypes.Select(infoType => new InformationTypeDto
                {
                    Id = infoType.Id,
                    Name = infoType.Name,
                    Description = infoType.Description
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de información");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de tipos de información", ex);
            }
        }

        /// <summary>
        /// Obtiene un tipo de información por su ID.
        /// </summary>
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

                return new InformationTypeDto
                {
                    Id = infoType.Id,
                    Name = infoType.Name,
                    Description = infoType.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tipo de información con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el tipo de información con ID {id}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo tipo de información en el sistema.
        /// </summary>
        public async Task<InformationTypeDto> CreateInformationTypeAsync(InformationTypeDto infoTypeDto)
        {
            ValidateInformationType(infoTypeDto);

            try
            {
                var infoType = new InformationType
                {
                    Name = infoTypeDto.Name,
                    Description = infoTypeDto.Description
                };

                var createdInfoType = await _infoTypeData.CreateAsync(infoType);
                return new InformationTypeDto
                {
                    Id = createdInfoType.Id,
                    Name = createdInfoType.Name,
                    Description = createdInfoType.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo tipo de información: {Name}", infoTypeDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el tipo de información", ex);
            }
        }

        /// <summary>
        /// Elimina un tipo de información por su ID.
        /// </summary>
        public async Task DeleteInformationTypeAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un tipo de información con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var infoType = await _infoTypeData.GetByIdAsync(id);
                if (infoType == null)
                {
                    _logger.LogInformation("No se encontró ningún tipo de información para eliminar con ID: {Id}", id);
                    throw new EntityNotFoundException("InformationType", id);
                }

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

        /// <summary>
        /// Valida los datos del tipo de información antes de su creación o actualización.
        /// </summary>
        private void ValidateInformationType(InformationTypeDto infoTypeDto)
        {
            if (infoTypeDto == null)
            {
                throw new ValidationException("El objeto tipo de información no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(infoTypeDto.Name))
            {
                _logger.LogWarning("Intento de crear un tipo de información con Name vacío");
                throw new ValidationException("Name", "El nombre del tipo de información es obligatorio");
            }
        }
    }
}
