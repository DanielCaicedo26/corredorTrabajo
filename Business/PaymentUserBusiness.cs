using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class PaymentUserBusiness
    {
        private readonly PaymentUserData _paymentUserData;
        private readonly ILogger<PaymentUserBusiness> _logger;

        public PaymentUserBusiness(PaymentUserData paymentUserData, ILogger<PaymentUserBusiness> logger)
        {
            _paymentUserData = paymentUserData;
            _logger = logger;
        }

        public async Task<IEnumerable<PaymentUserDto>> GetAllPaymentUsersAsync()
        {
            try
            {
                var users = await _paymentUserData.GetAllAsync();
                return users.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los PaymentUser");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de PaymentUser", ex);
            }
        }

        public async Task<PaymentUserDto> GetPaymentUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un PaymentUser con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var user = await _paymentUserData.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogInformation("No se encontró ningún PaymentUser con ID: {Id}", id);
                    throw new EntityNotFoundException("PaymentUser", id);
                }

                return MapToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el PaymentUser con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el PaymentUser con ID {id}", ex);
            }
        }

        public async Task<PaymentUserDto> CreatePaymentUserAsync(PaymentUserDto userDto)
        {
            try
            {
                ValidatePaymentUser(userDto);
                var user = MapToEntity(userDto);
                var createdUser = await _paymentUserData.CreateAsync(user);
                return MapToDto(createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo PaymentUser");
                throw new ExternalServiceException("Base de datos", "Error al crear el PaymentUser", ex);
            }
        }

        public async Task<PaymentUserDto> UpdatePaymentUserAsync(PaymentUserDto userDto)
        {
            try
            {
                ValidatePaymentUser(userDto);
                var user = MapToEntity(userDto);
                var updated = await _paymentUserData.UpdateAsync(user);
                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el PaymentUser");
                }
                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el PaymentUser");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el PaymentUser", ex);
            }
        }

        public async Task DeletePaymentUserAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un PaymentUser con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var deleted = await _paymentUserData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el PaymentUser");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el PaymentUser con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el PaymentUser con ID {id}", ex);
            }
        }

        public void ValidatePaymentUser(PaymentUserDto userDto)
        {
            if (userDto == null)
            {
                throw new ValidationException("El objeto PaymentUser no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(userDto.PaymentAgreement))
            {
                _logger.LogWarning("Se intentó crear/actualizar un PaymentUser con PaymentAgreement vacío");
                throw new ValidationException("PaymentAgreement", "El campo PaymentAgreement es obligatorio");
            }

            if (userDto.PersonId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un PaymentUser con PersonId inválido");
                throw new ValidationException("PersonId", "El PersonId debe ser mayor que cero");
            }
        }

        public PaymentUserDto MapToDto(PaymentUser user)
        {
            return new PaymentUserDto
            {
                Id = user.Id,
                PaymentAgreement = user.PaymentAgreement,
                PersonId = user.PersonId
            };
        }

        public PaymentUser MapToEntity(PaymentUserDto userDto)
        {
            return new PaymentUser
            {
                Id = userDto.Id,
                PaymentAgreement = userDto.PaymentAgreement,
                PersonId = userDto.PersonId
            };
        }
    }
}

