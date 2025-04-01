using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class PaymentHistoryBusiness
    {
        private readonly PaymentHistoryData _paymentHistoryData;
        private readonly ILogger<PaymentHistoryBusiness> _logger;

        public PaymentHistoryBusiness(PaymentHistoryData paymentHistoryData, ILogger<PaymentHistoryBusiness> logger)
        {
            _paymentHistoryData = paymentHistoryData;
            _logger = logger;
        }

        public async Task<IEnumerable<PaymentHistoryDto>> GetAllPaymentHistoriesAsync()
        {
            try
            {
                var payments = await _paymentHistoryData.GetAllAsync();
                return payments.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los historiales de pago");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de historiales de pago", ex);
            }
        }

        public async Task<PaymentHistoryDto> GetPaymentHistoryByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un historial de pago con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var payment = await _paymentHistoryData.GetByIdAsync(id);
                if (payment == null)
                {
                    _logger.LogInformation("No se encontró ningún historial de pago con ID: {Id}", id);
                    throw new EntityNotFoundException("PaymentHistory", id);
                }

                return MapToDto(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de pago con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el historial de pago con ID {id}", ex);
            }
        }

        public async Task<PaymentHistoryDto> CreatePaymentHistoryAsync(PaymentHistoryDto paymentDto)
        {
            try
            {
                ValidatePaymentHistory(paymentDto);

                var payment = MapToEntity(paymentDto);
                var createdPayment = await _paymentHistoryData.CreateAsync(payment);

                return MapToDto(createdPayment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo historial de pago");
                throw new ExternalServiceException("Base de datos", "Error al crear el historial de pago", ex);
            }
        }

        public async Task<PaymentHistoryDto> UpdatePaymentHistoryAsync(PaymentHistoryDto paymentDto)
        {
            try
            {
                ValidatePaymentHistory(paymentDto);

                var payment = MapToEntity(paymentDto);
                var updated = await _paymentHistoryData.UpdateAsync(payment);

                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el historial de pago");
                }

                return paymentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el historial de pago");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el historial de pago", ex);
            }
        }

        public async Task DeletePaymentHistoryAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un historial de pago con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var deleted = await _paymentHistoryData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el historial de pago");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el historial de pago con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el historial de pago con ID {id}", ex);
            }
        }

        private void ValidatePaymentHistory(PaymentHistoryDto paymentDto)
        {
            if (paymentDto == null)
            {
                throw new ValidationException("El objeto PaymentHistory no puede ser nulo");
            }

            if (paymentDto.PaymentDate == default)
            {
                _logger.LogWarning("Se intentó crear/actualizar un PaymentHistory con PaymentDate inválido");
                throw new ValidationException("PaymentDate", "La fecha de pago es obligatoria");
            }

            if (paymentDto.DiscountDate == default)
            {
                _logger.LogWarning("Se intentó crear/actualizar un PaymentHistory con DiscountDate inválido");
                throw new ValidationException("DiscountDate", "La fecha de descuento es obligatoria");
            }

            if (paymentDto.DiscountDate < paymentDto.PaymentDate)
            {
                _logger.LogWarning("La fecha de descuento no puede ser anterior a la fecha de pago");
                throw new ValidationException("DiscountDate", "La fecha de descuento no puede ser anterior a la fecha de pago");
            }
        }

        private static PaymentHistoryDto MapToDto(PaymentHistory payment) => new PaymentHistoryDto
        {
            Id = payment.Id,
            PaymentDate = payment.Paymentdate,
            DiscountDate = payment.Discountdate
        };

        private static PaymentHistory MapToEntity(PaymentHistoryDto paymentDto) => new PaymentHistory
        {
            Id = paymentDto.Id,
            Paymentdate = paymentDto.PaymentDate,
            Discountdate = paymentDto.DiscountDate
        };
    }
}
