using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los tipos de pago del sistema.
    /// </summary>
    public class TypePaymentBussines
    {
        private readonly TypePaymentData _typePaymentData;
        private readonly ILogger _logger;

        public TypePaymentBussines(TypePaymentData typePaymentData, ILogger logger)
        {
            _typePaymentData = typePaymentData;
            _logger = logger;
        }

        // Método para obtener todos los tipos de pago como DTOs
        public async Task<IEnumerable<TypePaymentDto>> GetAllTypePaymentsAsync()
        {
            try
            {
                var typePayments = await _typePaymentData.GetAllAsync();
                var typePaymentsDTO = new List<TypePaymentDto>();

                foreach (var typePayment in typePayments)
                {
                    typePaymentsDTO.Add(new TypePaymentDto
                    {
                        Id = typePayment.Id,
                        Name = typePayment.Name,
                        Description = typePayment.Description,
                        Payments = typePayment.Payments
                    });
                }

                return typePaymentsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de pago");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de tipos de pago", ex);
            }
        }

        // Método para obtener un tipo de pago por ID como DTO
        public async Task<TypePaymentDto> GetTypePaymentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un tipo de pago con ID inválido: {TypePaymentId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del tipo de pago debe ser mayor que cero");
            }

            try
            {
                var typePayment = await _typePaymentData.GetByIdAsync(id);
                if (typePayment == null)
                {
                    _logger.LogInformation("No se encontró ningún tipo de pago con ID: {TypePaymentId}", id);
                    throw new EntityNotFoundException("TypePayment", id);
                }

                return new TypePaymentDto
                {
                    Id = typePayment.Id,
                    Name = typePayment.Name,
                    Description = typePayment.Description,
                    Payments = typePayment.Payments
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tipo de pago con ID: {TypePaymentId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el tipo de pago con ID {id}", ex);
            }
        }

        // Método para crear un tipo de pago desde un DTO
        public async Task<TypePaymentDto> CreateTypePaymentAsync(TypePaymentDto typePaymentDto)
        {
            try
            {
                ValidateTypePayment(typePaymentDto);

                var typePayment = new TypePayment
                {
                    Name = typePaymentDto.Name,
                    Description = typePaymentDto.Description,
                    Payments = typePaymentDto.Payments
                };

                var typePaymentCreado = await _typePaymentData.CreateAsync(typePayment);

                return new TypePaymentDto
                {
                    Id = typePaymentCreado.Id,
                    Name = typePaymentCreado.Name,
                    Description = typePaymentCreado.Description,
                    Payments = typePaymentCreado.Payments
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo tipo de pago: {TypePaymentNombre}", typePaymentDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el tipo de pago", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateTypePayment(TypePaymentDto typePaymentDto)
        {
            if (typePaymentDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto tipo de pago no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(typePaymentDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un tipo de pago con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del tipo de pago es obligatorio");
            }
        }
    }
}
