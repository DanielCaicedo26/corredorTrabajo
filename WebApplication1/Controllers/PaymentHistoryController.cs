


using Business;
using Entity.Dto;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión del historial de pagos en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PaymentHistoryController : ControllerBase
    {
        private readonly PaymentHistoryBusiness _paymentHistoryBusiness;
        private readonly ILogger<PaymentHistoryController> _logger;

        public PaymentHistoryController(PaymentHistoryBusiness paymentHistoryBusiness, ILogger<PaymentHistoryController> logger)
        {
            _paymentHistoryBusiness = paymentHistoryBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los registros del historial de pagos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentHistoryDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<PaymentHistoryDto>>> GetAllPaymentHistories()
        {
            try
            {
                var paymentHistories = await _paymentHistoryBusiness.GetAllPaymentHistoriesAsync();
                return Ok(paymentHistories);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener historial de pagos");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener historial de pagos");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Obtiene un historial de pago específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentHistoryDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentHistoryById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID debe ser un número positivo." });
            }

            try
            {
                var paymentHistory = await _paymentHistoryBusiness.GetPaymentHistoryByIdAsync(id);
                return Ok(paymentHistory);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el historial de pago con ID: {PaymentHistoryId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Historial de pago no encontrado con ID: {PaymentHistoryId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener historial de pago con ID: {PaymentHistoryId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener historial de pago con ID: {PaymentHistoryId}", id);
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Crea un nuevo registro en el historial de pagos
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PaymentHistoryDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePaymentHistory([FromBody] PaymentHistoryDto paymentHistoryDto)
        {
            if (paymentHistoryDto == null)
            {
                return BadRequest(new { message = "Los datos del historial de pago son obligatorios." });
            }

            try
            {
                var createdPaymentHistory = await _paymentHistoryBusiness.CreatePaymentHistoryAsync(paymentHistoryDto);
                return CreatedAtAction(nameof(GetPaymentHistoryById), new { id = createdPaymentHistory.Id }, createdPaymentHistory);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear historial de pago");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear historial de pago");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear historial de pago");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }
    }
}
