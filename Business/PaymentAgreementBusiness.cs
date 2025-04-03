using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    public class PaymentAgreementBusiness
    {
        private readonly PaymentAgreementData _paymentAgreementData;
        private readonly ILogger<PaymentAgreementBusiness> _logger;

        public PaymentAgreementBusiness(PaymentAgreementData paymentAgreementData, ILogger<PaymentAgreementBusiness> logger)
        {
            _paymentAgreementData = paymentAgreementData;
            _logger = logger;
        }

        public async Task<IEnumerable<PaymentAgreementDto>> GetAllPaymentAgreementsAsync()
        {
            try
            {
                var agreements = await _paymentAgreementData.GetAllAsync();
                return agreements.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los acuerdos de pago");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de acuerdos de pago", ex);
            }
        }

        public async Task<PaymentAgreementDto> GetPaymentAgreementByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un acuerdo de pago con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var agreement = await _paymentAgreementData.GetByIdAsync(id);
                if (agreement == null)
                {
                    _logger.LogInformation("No se encontró ningún acuerdo de pago con ID: {Id}", id);
                    throw new EntityNotFoundException("PaymentAgreement", id);
                }

                return MapToDto(agreement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el acuerdo de pago con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el acuerdo de pago con ID {id}", ex);
            }
        }

        public async Task<PaymentAgreementDto> CreatePaymentAgreementAsync(PaymentAgreementDto agreementDto)
        {
            try
            {
                ValidatePaymentAgreement(agreementDto);

                var agreement = MapToEntity(agreementDto);
                var createdAgreement = await _paymentAgreementData.CreateAsync(agreement);

                return MapToDto(createdAgreement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo acuerdo de pago");
                throw new ExternalServiceException("Base de datos", "Error al crear el acuerdo de pago", ex);
            }
        }

        public async Task<PaymentAgreementDto> UpdatePaymentAgreementAsync(PaymentAgreementDto agreementDto)
        {
            try
            {
                ValidatePaymentAgreement(agreementDto);

                var agreement = MapToEntity(agreementDto);
                var updated = await _paymentAgreementData.UpdateAsync(agreement);

                if (!updated)
                {
                    throw new ExternalServiceException("Base de datos", "Error al actualizar el acuerdo de pago");
                }

                return agreementDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el acuerdo de pago");
                throw new ExternalServiceException("Base de datos", "Error al actualizar el acuerdo de pago", ex);
            }
        }

        public async Task DeletePaymentAgreementAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un acuerdo de pago con ID inválido: {Id}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var deleted = await _paymentAgreementData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el acuerdo de pago");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el acuerdo de pago con ID: {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el acuerdo de pago con ID {id}", ex);
            }
        }

        private void ValidatePaymentAgreement(PaymentAgreementDto agreementDto)
        {
            if (agreementDto == null)
            {
                throw new ValidationException("El objeto PaymentAgreement no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(agreementDto.Address))
            {
                _logger.LogWarning("Se intentó crear/actualizar un PaymentAgreement con Address vacío");
                throw new ValidationException("Address", "La dirección es obligatoria");
            }

            if (string.IsNullOrWhiteSpace(agreementDto.Neighborhood))
            {
                _logger.LogWarning("Se intentó crear/actualizar un PaymentAgreement con Neighborhood vacío");
                throw new ValidationException("Neighborhood", "El barrio es obligatorio");
            }

            if (agreementDto.FinanceAmount <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un PaymentAgreement con FinanceAmount inválido");
                throw new ValidationException("FinanceAmount", "El monto financiero debe ser mayor que cero");
            }

            if (string.IsNullOrWhiteSpace(agreementDto.AgreementDescription))
            {
                _logger.LogWarning("Se intentó crear/actualizar un PaymentAgreement con AgreementDescription vacío");
                throw new ValidationException("AgreementDescription", "La descripción del acuerdo es obligatoria");
            }
        }

        private static PaymentAgreementDto MapToDto(PaymentAgreement agreement) => new PaymentAgreementDto
        {
            Id = agreement.Id,
            Address = agreement.Address,
            Neighborhood = agreement.Neighborhood,
            FinanceAmount = agreement.Financeamount,
            AgreementDescription = agreement.AgreementaDescription
        };

        private static PaymentAgreement MapToEntity(PaymentAgreementDto agreementDto) => new PaymentAgreement
        {
            Id = agreementDto.Id,
            Address = agreementDto.Address,
            Neighborhood = agreementDto.Neighborhood,
            Financeamount = agreementDto.FinanceAmount,
            AgreementaDescription = agreementDto.AgreementDescription
        };
    }
}
