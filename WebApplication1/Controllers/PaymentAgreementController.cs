using Business;
using Entity.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de acuerdos de pago
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PaymentAgreementController : ControllerBase
    {
        private readonly PaymentAgreementBusiness _paymentAgreementBusiness;
        private readonly ILogger<PaymentAgreementController> _logger;

        public PaymentAgreementController(PaymentAgreementBusiness paymentAgreementBusiness, ILogger<PaymentAgreementController> logger)
        {
            _paymentAgreementBusiness = paymentAgreementBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los acuerdos de pago del sistema
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentAgreementDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<PaymentAgreementDto>>> GetAllPaymentAgreements()
        {
            try
            {
                var agreements = await _paymentAgreementBusiness.GetAllPaymentAgreementsAsync();
                return Ok(agreements);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener acuerdos de pago");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener acuerdos de pago");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Obtiene un acuerdo de pago específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentAgreementDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentAgreementById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID debe ser un número positivo." });
            }

            try
            {
                var agreement = await _paymentAgreementBusiness.GetPaymentAgreementByIdAsync(id);
                return Ok(agreement);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el acuerdo de pago con ID: {PaymentAgreementId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Acuerdo de pago no encontrado con ID: {PaymentAgreementId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener acuerdo de pago con ID: {PaymentAgreementId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener acuerdo de pago con ID: {PaymentAgreementId}", id);
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Crea un nuevo acuerdo de pago en el sistema
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PaymentAgreementDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePaymentAgreement([FromBody] PaymentAgreementDto paymentAgreementDto)
        {
            if (paymentAgreementDto == null)
            {
                return BadRequest(new { message = "Los datos del acuerdo de pago son obligatorios." });
            }

            try
            {
                var createdAgreement = await _paymentAgreementBusiness.CreatePaymentAgreementAsync(paymentAgreementDto);
                return CreatedAtAction(nameof(GetPaymentAgreementById), new { id = createdAgreement.Id }, createdAgreement);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear acuerdo de pago");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear acuerdo de pago");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear acuerdo de pago");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }
    }
}
