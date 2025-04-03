using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;


namespace Business
{
    public class TypeInfractionBusiness
    {
        private readonly TypeInfractionData _typeInfractionData;
        private readonly ILogger _logger;

        public TypeInfractionBusiness(TypeInfractionData typeInfractionData, ILogger logger)
        {
            _typeInfractionData = typeInfractionData;
            _logger = logger;
        }

        public async Task<IEnumerable<TypeInfractionDto>> GetAllAsync()
        {
            try
            {
                var infractions = await _typeInfractionData.GetAllAsync();
                var infractionsDto = new List<TypeInfractionDto>();

                foreach (var infraction in infractions)
                {
                    infractionsDto.Add(new TypeInfractionDto
                    {
                        Id = infraction.Id,
                        TypeViolation = infraction.TypeViolation,
                        Description = infraction.Description,
                        ValueViolation = infraction.ValueViolation,
                        UserNotificationId = infraction.UserNotificationId,
                        UserNotification = infraction.UserNotification
                    });
                }

                return infractionsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las infracciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de infracciones", ex);
            }
        }

        public async Task<TypeInfractionDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una infracción con un ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var infraction = await _typeInfractionData.GetByIdAsync(id);
                if (infraction == null)
                {
                    _logger.LogInformation("No se encontró ninguna infracción con ID: {Id}", id);
                    throw new EntityNotFoundException("Infracción", id);
                }

                return new TypeInfractionDto
                {
                    Id = infraction.Id,
                    TypeViolation = infraction.TypeViolation,
                    Description = infraction.Description,
                    ValueViolation = infraction.ValueViolation,
                    UserNotificationId = infraction.UserNotificationId,
                    UserNotification = infraction.UserNotification
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la infracción con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la infracción con ID {id}", ex);
            }
        }

        public async Task<TypeInfractionDto> CreateAsync(TypeInfractionDto infractionDto)
        {
            try
            {
                ValidateInfraction(infractionDto);

                var infraction = new TypeInfraction
                {
                    TypeViolation = infractionDto.TypeViolation,
                    Description = infractionDto.Description,
                    ValueViolation = infractionDto.ValueViolation,
                    UserNotificationId = infractionDto.UserNotificationId,
                    UserNotification = infractionDto.UserNotification
                };

                var createdInfraction = await _typeInfractionData.CreateAsync(infraction);

                return new TypeInfractionDto
                {
                    Id = createdInfraction.Id,
                    TypeViolation = createdInfraction.TypeViolation,
                    Description = createdInfraction.Description,
                    ValueViolation = createdInfraction.ValueViolation,
                    UserNotificationId = createdInfraction.UserNotificationId,
                    UserNotification = createdInfraction.UserNotification
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva infracción: {Description}", infractionDto?.Description ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la infracción", ex);
            }
        }

        private void ValidateInfraction(TypeInfractionDto infractionDto)
        {
            if (infractionDto == null)
            {
                throw new ValidationException("El objeto infracción no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(infractionDto.TypeViolation))
            {
                _logger.LogWarning("Se intentó crear/actualizar una infracción sin tipo de violación");
                throw new ValidationException("TypeViolation", "El tipo de violación es obligatorio");
            }
        }
    }
}
