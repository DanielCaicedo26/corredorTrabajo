using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class InformationInfractionBusiness
    {
        private readonly InformationInfractionData _infractionData;
        private readonly ILogger<InformationInfractionBusiness> _logger;

        public InformationInfractionBusiness(InformationInfractionData infractionData, ILogger<InformationInfractionBusiness> logger)
        {
            _infractionData = infractionData;
            _logger = logger;
        }

        public async Task<IEnumerable<InformationInfractionDto>> GetAllInfractionsAsync()
        {
            try
            {
                var infractions = await _infractionData.GetAllAsync();
                return infractions.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las infracciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de infracciones", ex);
            }
        }

        public async Task<InformationInfractionDto> GetInfractionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una infracción con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID de la infracción debe ser mayor que cero");
            }

            try
            {
                var infraction = await _infractionData.GetByIdAsync(id);
                if (infraction == null)
                {
                    _logger.LogInformation("No se encontró ninguna infracción con ID: {Id}", id);
                    throw new EntityNotFoundException("InformationInfraction", id);
                }

                return MapToDto(infraction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la infracción con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la infracción con ID {id}", ex);
            }
        }

        public async Task<InformationInfractionDto> CreateInfractionAsync(InformationInfractionDto infractionDto)
        {
            try
            {
                ValidateInfraction(infractionDto);

                var infraction = MapToEntity(infractionDto);
                var createdInfraction = await _infractionData.CreateAsync(infraction);

                return MapToDto(createdInfraction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva infracción");
                throw new ExternalServiceException("Base de datos", "Error al crear la infracción", ex);
            }
        }

        public async Task<InformationInfractionDto> UpdateInfractionAsync(InformationInfractionDto infractionDto)
        {
            try
            {
                ValidateInfraction(infractionDto);

                var infraction = MapToEntity(infractionDto);
                var updated = await _infractionData.UpdateAsync(infraction);

                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar la infracción");
                }

                return infractionDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la infracción");
                throw new ExternalServiceException("Base de datos", "Error al actualizar la infracción", ex);
            }
        }

        public async Task DeleteInfractionAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar una infracción con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID de la infracción debe ser mayor que cero");
            }

            try
            {
                var deleted = await _infractionData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar la infracción");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la infracción con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar la infracción con ID {id}", ex);
            }
        }

        private void ValidateInfraction(InformationInfractionDto infractionDto)
        {
            if (infractionDto == null)
            {
                throw new ValidationException("El objeto infracción no puede ser nulo");
            }

            if (infractionDto.NumberSMLDV <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar una infracción con NumberSMLDV inválido: {NumberSMLDV}", infractionDto.NumberSMLDV);
                throw new ValidationException("NumberSMLDV", "El número de SMLDV debe ser mayor que cero");
            }

            if (infractionDto.MinimumWage <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar una infracción con MinimumWage inválido: {MinimumWage}", infractionDto.MinimumWage);
                throw new ValidationException("MinimumWage", "El salario mínimo debe ser mayor que cero");
            }
        }

        private static InformationInfractionDto MapToDto(InformationInfraction infraction) => new InformationInfractionDto
        {
            Id = infraction.Id,
            NumberSMLDV = infraction.NumberSMLDV,
            MinimumWage = infraction.MinimumWage,
            ValueSMLDV = infraction.ValueSMLDV,
            TotalValue = infraction.TotalValue
        };

        private static InformationInfraction MapToEntity(InformationInfractionDto infractionDto) => new InformationInfraction
        {
            Id = infractionDto.Id,
            NumberSMLDV = infractionDto.NumberSMLDV,
            MinimumWage = infractionDto.MinimumWage
        };
    }
}
