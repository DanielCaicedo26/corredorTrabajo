using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class PaymentBusiness
    {
        private readonly PaymentData _paymentData;
        private readonly ILogger<PaymentBusiness> _logger;

        public PaymentBusiness(PaymentData paymentData, ILogger<PaymentBusiness> logger)
        {
            _paymentData = paymentData;
            _logger = logger;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            try
            {
                var payments = await _paymentData.GetAllAsync();
                return payments.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los pagos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de pagos", ex);
            }
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un pago con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var payment = await _paymentData.GetByIdAsync(id);
                if (payment == null)
                {
                    _logger.LogInformation("No se encontró ningún pago con ID: {Id}", id);
                    throw new EntityNotFoundException("Payment", id);
                }

                return MapToDto(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el pago con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el pago con ID {id}", ex);
            }
        }

        public async Task<PaymentDto> CreatePaymentAsync(PaymentDto paymentDto)
        {
            try
            {
                ValidatePayment(paymentDto);

                var payment = MapToEntity(paymentDto);
                var createdPayment = await _paymentData.CreateAsync(payment);

                return MapToDto(createdPayment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo pago");
                throw new ExternalServiceException("Base de datos", "Error al crear el pago", ex);
            }
        }

        public async Task<PaymentDto> UpdatePaymentAsync(PaymentDto paymentDto)
        {
            try
            {
                ValidatePayment(paymentDto);

                var payment = MapToEntity(paymentDto);
                var updated = await _paymentData.UpdateAsync(payment);

                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el pago");
                }

                return paymentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el pago");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el pago", ex);
            }
        }

        public async Task DeletePaymentAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un pago con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var deleted = await _paymentData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el pago");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el pago con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el pago con ID {id}", ex);
            }
        }

        private void ValidatePayment(PaymentDto paymentDto)
        {
            if (paymentDto == null)
            {
                throw new ValidationException("El objeto Payment no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(paymentDto.TypePayment))
            {
                _logger.LogWarning("Se intentó crear/actualizar un pago con TypePayment vacío");
                throw new ValidationException("TypePayment", "El TypePayment es obligatorio");
            }

            if (paymentDto.UserViolationId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un pago con UserViolationId inválido");
                throw new ValidationException("UserViolationId", "El UserViolationId debe ser mayor que cero");
            }
        }

        private static PaymentDto MapToDto(Payment payment) => new PaymentDto
        {
            Id = payment.Id,
            TypePayment = payment.TypePayment,
            UserViolationId = payment.UserViolationId
        };

        private static Payment MapToEntity(PaymentDto paymentDto) => new Payment
        {
            Id = paymentDto.Id,
            TypePayment = paymentDto.TypePayment,
            UserViolationId = paymentDto.UserViolationId
        };
    }
}
