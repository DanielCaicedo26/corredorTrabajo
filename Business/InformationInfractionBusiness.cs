using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las infracciones de información.
    /// </summary>
    public class InformationInfractionBusiness
    {
        private readonly InformationInfractionData _infractionData;
        private readonly ILogger<InformationInfractionBusiness> _logger;

        public InformationInfractionBusiness(InformationInfractionData infractionData, ILogger<InformationInfractionBusiness> logger)
        {
            _infractionData = infractionData;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las infracciones registradas en el sistema.
        /// </summary>
        /// <returns>Lista de infracciones como DTOs</returns>
        public async Task<IEnumerable<InformationInfractionDto>> GetAllInfractionsAsync()
        {
            try
            {
                var infractions = await _infractionData.GetAllAsync();
                return infractions.Select(infraction => new InformationInfractionDto
                {
                    Id = infraction.Id,
                    NumberSMLDV = infraction.NumberSMLDV,
                    MinimumWage = infraction.MinimumWage,
                    ValueSMLDV = infraction.ValueSMLDV,
                    TotalValue = infraction.TotalValue
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las infracciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de infracciones", ex);
            }
        }

        /// <summary>
        /// Obtiene una infracción por su ID.
        /// </summary>
        /// <param name="id">ID de la infracción</param>
        /// <returns>Infracción encontrada</returns>
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

                return new InformationInfractionDto
                {
                    Id = infraction.Id,
                    NumberSMLDV = infraction.NumberSMLDV,
                    MinimumWage = infraction.MinimumWage,
                    ValueSMLDV = infraction.ValueSMLDV,
                    TotalValue = infraction.TotalValue
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la infracción con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la infracción con ID {id}", ex);
            }
        }

        /// <summary>
        /// Crea una nueva infracción en el sistema.
        /// </summary>
        /// <param name="infractionDto">Datos de la infracción a crear</param>
        /// <returns>Infracción creada</returns>
        public async Task<InformationInfractionDto> CreateInfractionAsync(InformationInfractionDto infractionDto)
        {
            ValidateInfraction(infractionDto);

            try
            {
                var infraction = new InformationInfraction
                {
                    NumberSMLDV = infractionDto.NumberSMLDV,
                    MinimumWage = infractionDto.MinimumWage
                };

                var createdInfraction = await _infractionData.CreateAsync(infraction);
                return new InformationInfractionDto
                {
                    Id = createdInfraction.Id,
                    NumberSMLDV = createdInfraction.NumberSMLDV,
                    MinimumWage = createdInfraction.MinimumWage,
                    ValueSMLDV = createdInfraction.ValueSMLDV,
                    TotalValue = createdInfraction.TotalValue
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva infracción");
                throw new ExternalServiceException("Base de datos", "Error al crear la infracción", ex);
            }
        }

        /// <summary>
        /// Elimina una infracción por su ID.
        /// </summary>
        /// <param name="id">ID de la infracción a eliminar</param>
        public async Task DeleteInfractionAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar una infracción con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID de la infracción debe ser mayor que cero");
            }

            try
            {
                var infraction = await _infractionData.GetByIdAsync(id);
                if (infraction == null)
                {
                    _logger.LogInformation("No se encontró ninguna infracción para eliminar con ID: {Id}", id);
                    throw new EntityNotFoundException("InformationInfraction", id);
                }

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

        /// <summary>
        /// Valida los datos de la infracción antes de su creación o actualización.
        /// </summary>
        /// <param name="infractionDto">Objeto InformationInfractionDto a validar</param>
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
        }
    }
}
