using Business;
using Entity.Dto;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de infracciones en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InformationInfractionController : ControllerBase
    {
        private readonly InformationInfractionBusiness _infractionBusiness;
        private readonly ILogger<InformationInfractionController> _logger;

        /// <summary>
        /// Constructor del controlador de infracciones
        /// </summary>
        /// <param name="infractionBusiness">Capa de negocio de infracciones</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public InformationInfractionController(InformationInfractionBusiness infractionBusiness, ILogger<InformationInfractionController> logger)
        {
            _infractionBusiness = infractionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las infracciones del sistema
        /// </summary>
        /// <returns>Lista de infracciones</returns>
        /// <response code="200">Retorna la lista de infracciones</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InformationInfractionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllInfractions()
        {
            try
            {
                var infractions = await _infractionBusiness.GetAllInfractionsAsync();
                return Ok(infractions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener infracciones");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una infracción específica por su ID
        /// </summary>
        /// <param name="id">ID de la infracción</param>
        /// <returns>Infracción solicitada</returns>
        /// <response code="200">Retorna la infracción solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Infracción no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InformationInfractionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInfractionById(int id)
        {
            try
            {
                var infraction = await _infractionBusiness.GetInfractionByIdAsync(id);
                return Ok(infraction);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la infracción con ID: {InfractionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Infracción no encontrada con ID: {InfractionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener infracción con ID: {InfractionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva infracción en el sistema
        /// </summary>
        /// <param name="infractionDto">Datos de la infracción a crear</param>
        /// <returns>Infracción creada</returns>
        /// <response code="201">Retorna la infracción creada</response>
        /// <response code="400">Datos de la infracción no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(InformationInfractionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateInfraction([FromBody] InformationInfractionDto infractionDto)
        {
            try
            {
                var createdInfraction = await _infractionBusiness.CreateInfractionAsync(infractionDto);
                return CreatedAtAction(nameof(GetInfractionById), new { id = createdInfraction.Id }, createdInfraction);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear infracción");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear infracción");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
