using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las infracciones de usuario del sistema.
    /// </summary>
    public class UserInfractionBussines
    {
        private readonly UserInfractionData _userInfractionData;
        private readonly ILogger _logger;

        public UserInfractionBussines(UserInfractionData userInfractionData, ILogger logger)
        {
            _userInfractionData = userInfractionData;
            _logger = logger;
        }

        // Método para obtener todas las infracciones de usuario como DTOs
        public async Task<IEnumerable<UserInfractionDto>> GetAllUserInfractionsAsync()
        {
            try
            {
                var userInfractions = await _userInfractionData.GetAllAsync();
                var userInfractionsDTO = new List<UserInfractionDto>();

                foreach (var userInfraction in userInfractions)
                {
                    userInfractionsDTO.Add(new UserInfractionDto
                    {
                        Id = userInfraction.Id,
                        TypeInfractionId = userInfraction.TypeInfractionId,
                        TypeInfraction = userInfraction.TypeInfraction
                    });
                }

                return userInfractionsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las infracciones de usuario");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de infracciones de usuario", ex);
            }
        }

        // Método para obtener una infracción de usuario por ID como DTO
        public async Task<UserInfractionDto> GetUserInfractionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una infracción de usuario con ID inválido: {UserInfractionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la infracción de usuario debe ser mayor que cero");
            }

            try
            {
                var userInfraction = await _userInfractionData.GetByIdAsync(id);
                if (userInfraction == null)
                {
                    _logger.LogInformation("No se encontró ninguna infracción de usuario con ID: {UserInfractionId}", id);
                    throw new EntityNotFoundException("UserInfraction", id);
                }

                return new UserInfractionDto
                {
                    Id = userInfraction.Id,
                    TypeInfractionId = userInfraction.TypeInfractionId,
                    TypeInfraction = userInfraction.TypeInfraction
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la infracción de usuario con ID: {UserInfractionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la infracción de usuario con ID {id}", ex);
            }
        }

        // Método para crear una infracción de usuario desde un DTO
        public async Task<UserInfractionDto> CreateUserInfractionAsync(UserInfractionDto userInfractionDto)
        {
            try
            {
                ValidateUserInfraction(userInfractionDto);

                var userInfraction = new UserInfraction
                {
                    TypeInfractionId = userInfractionDto.TypeInfractionId,
                    TypeInfraction = userInfractionDto.TypeInfraction
                };

                var userInfractionCreado = await _userInfractionData.CreateAsync(userInfraction);

                return new UserInfractionDto
                {
                    Id = userInfractionCreado.Id,
                    TypeInfractionId = userInfractionCreado.TypeInfractionId,
                    TypeInfraction = userInfractionCreado.TypeInfraction
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva infracción de usuario: {UserInfractionId}", userInfractionDto?.Id ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la infracción de usuario", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateUserInfraction(UserInfractionDto userInfractionDto)
        {
            if (userInfractionDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto infracción de usuario no puede ser nulo");
            }

            if (userInfractionDto.TypeInfractionId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar una infracción de usuario con TypeInfractionId inválido");
                throw new Utilities.Exceptions.ValidationException("TypeInfractionId", "El TypeInfractionId de la infracción de usuario es obligatorio y debe ser mayor que cero");
            }
        }
    }
}
