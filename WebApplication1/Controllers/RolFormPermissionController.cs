using Business;
using Entity.Dto;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de relaciones entre roles, formularios y permisos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolFormPermissionController : ControllerBase
    {
        private readonly RolFormPermissionBusiness _rolFormPermissionBusiness;
        private readonly ILogger<RolFormPermissionController> _logger;

        public RolFormPermissionController(RolFormPermissionBusiness rolFormPermissionBusiness, ILogger<RolFormPermissionController> logger)
        {
            _rolFormPermissionBusiness = rolFormPermissionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las relaciones Rol-Form-Permiso.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolFormPermissionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _rolFormPermissionBusiness.GetAllAsync();
                return Ok(result);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener todas las relaciones Rol-Form-Permiso");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener todas las relaciones Rol-Form-Permiso");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Obtiene una relación Rol-Form-Permiso por su ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolFormPermissionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "El ID debe ser un número positivo." });

            try
            {
                var result = await _rolFormPermissionBusiness.GetByIdAsync(id);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la relación Rol-Form-Permiso con ID: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Relación Rol-Form-Permiso no encontrada con ID: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener la relación Rol-Form-Permiso con ID: {Id}", id);
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener la relación Rol-Form-Permiso con ID: {Id}", id);
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Crea una nueva relación Rol-Form-Permiso.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(RolFormPermissionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromBody] RolFormPermissionDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Los datos de la relación son obligatorios." });

            try
            {
                var created = await _rolFormPermissionBusiness.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear relación Rol-Form-Permiso");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear relación Rol-Form-Permiso");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear relación Rol-Form-Permiso");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }
    }
}
