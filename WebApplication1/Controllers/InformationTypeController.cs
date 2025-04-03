using Business;
using Entity.Dto;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

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

        /// <summary>
        /// Constructor del controlador de tipos de información
        /// </summary>
        /// <param name="typeBusiness">Capa de negocio de tipos de información</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public InformationTypeController(InformationTypeBusiness typeBusiness, ILogger<InformationTypeController> logger)
        {
            _typeBusiness = typeBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los tipos de información del sistema
        /// </summary>
        /// <returns>Lista de tipos de información</returns>
        /// <response code="200">Retorna la lista de tipos de información</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InformationTypeDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllInformationTypes()
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
        }

        /// <summary>
        /// Obtiene un tipo de información específico por su ID
        /// </summary>
        /// <param name="id">ID del tipo de información</param>
        /// <returns>Tipo de información solicitado</returns>
        /// <response code="200">Retorna el tipo de información solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Tipo de información no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InformationTypeDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInformationTypeById(int id)
        {
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
        }

        /// <summary>
        /// Crea un nuevo tipo de información en el sistema
        /// </summary>
        /// <param name="typeDto">Datos del tipo de información a crear</param>
        /// <returns>Tipo de información creado</returns>
        /// <response code="201">Retorna el tipo de información creado</response>
        /// <response code="400">Datos del tipo de información no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(InformationTypeDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateInformationType([FromBody] InformationTypeDto typeDto)
        {
            try
            {
                var createdType = await _typeBusiness.CreateInformationTypeAsync(typeDto);
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
        }
    }
}
