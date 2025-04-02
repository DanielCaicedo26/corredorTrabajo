using Business;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using utilities.Exeptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de tipos de información en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InformationTypeController : ControllerBase
    {
        private readonly InformationTypeBusiness _typeBusiness;
        private readonly ILogger<InformationTypeController> _logger;

        public InformationTypeController(InformationTypeBusiness typeBusiness, ILogger<InformationTypeController> logger)
        {
            _typeBusiness = typeBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los tipos de información del sistema
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InformationType>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<InformationType>>> GetAllInformationTypes()
        {
            try
            {
                var types = await _typeBusiness.GetAllInformationTypesAsync();
                return Ok(types);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener tipos de información");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener tipos de información");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Obtiene un tipo de información específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InformationType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInformationTypeById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID debe ser un número positivo." });
            }

            try
            {
                var type = await _typeBusiness.GetInformationTypeByIdAsync(id);
                return Ok(type);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el tipo de información con ID: {TypeId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Tipo de información no encontrado con ID: {TypeId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener tipo de información con ID: {TypeId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener tipo de información con ID: {TypeId}", id);
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Crea un nuevo tipo de información en el sistema
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(InformationType), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateInformationType([FromBody] InformationType type)
        {
            if (type == null)
            {
                return BadRequest(new { message = "Los datos del tipo de información son obligatorios." });
            }

            try
            {
                var createdType = await _typeBusiness.CreateInformationTypeAsync(type);
                return CreatedAtAction(nameof(GetInformationTypeById), new { id = createdType.Id }, createdType);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear tipo de información");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear tipo de información");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear tipo de información");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }
    }
}
