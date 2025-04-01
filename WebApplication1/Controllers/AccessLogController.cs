using Business;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de registros de acceso en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccessLogController : ControllerBase
    {
        private readonly AccessLogBusiness _accessLogBusiness;
        private readonly ILogger<AccessLogController> _logger;

        public AccessLogController(AccessLogBusiness accessLogBusiness, ILogger<AccessLogController> logger)
        {
            _accessLogBusiness = accessLogBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los registros de acceso del sistema
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccessLog>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AccessLog>>> GetAllAccessLogs()
        {
            try
            {
                var logs = await _accessLogBusiness.GetAllAccessLogsAsync();
                return Ok(logs);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener registros de acceso");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener registros de acceso");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Obtiene un registro de acceso específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AccessLog), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAccessLogById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID debe ser un número positivo." });
            }

            try
            {
                var log = await _accessLogBusiness.GetAccessLogByIdAsync(id);
                return Ok(log);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el registro de acceso con ID: {LogId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Registro de acceso no encontrado con ID: {LogId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener registro de acceso con ID: {LogId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener registro de acceso con ID: {LogId}", id);
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Crea un nuevo registro de acceso en el sistema
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AccessLog), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateAccessLog([FromBody] AccessLog accessLog)
        {
            if (accessLog == null)
            {
                return BadRequest(new { message = "Los datos del registro de acceso son obligatorios." });
            }

            try
            {
                var createdLog = await _accessLogBusiness.CreateAccessLogAsync(accessLog);
                return CreatedAtAction(nameof(GetAccessLogById), new { id = createdLog.Id }, createdLog);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear registro de acceso");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear registro de acceso");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear registro de acceso");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }
    }
}
