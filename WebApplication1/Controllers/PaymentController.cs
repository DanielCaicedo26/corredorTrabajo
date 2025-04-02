
using Business;
using Entity.Dto;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de pagos en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentBusiness _paymentBusiness;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(PaymentBusiness paymentBusiness, ILogger<PaymentController> logger)
        {
            _paymentBusiness = paymentBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los pagos del sistema
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAllPayments()
        {
            try
            {
                var payments = await _paymentBusiness.GetAllPaymentsAsync();
                return Ok(payments);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener pagos");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener pagos");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Obtiene un pago específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID debe ser un número positivo." });
            }

            try
            {
                var payment = await _paymentBusiness.GetPaymentByIdAsync(id);
                return Ok(payment);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el pago con ID: {PaymentId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Pago no encontrado con ID: {PaymentId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener pago con ID: {PaymentId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener pago con ID: {PaymentId}", id);
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Crea un nuevo pago en el sistema
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PaymentDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto paymentDto)
        {
            if (paymentDto == null)
            {
                return BadRequest(new { message = "Los datos del pago son obligatorios." });
            }

            try
            {
                var createdPayment = await _paymentBusiness.CreatePaymentAsync(paymentDto);
                return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.Id }, createdPayment);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear pago");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear pago");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear pago");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }
    }
}
