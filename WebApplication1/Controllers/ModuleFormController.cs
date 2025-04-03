using Business;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

using CustomValidationException = Utilities.Exceptions.ValidationException;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de relaciones entre módulos y formularios
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ModuleFormController : ControllerBase
    {
        private readonly ModuleFormBusiness _moduleFormBusiness;
        private readonly ILogger<ModuleFormController> _logger;

        public ModuleFormController(ModuleFormBusiness moduleFormBusiness, ILogger<ModuleFormController> logger)
        {
            _moduleFormBusiness = moduleFormBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las relaciones entre módulos y formularios del sistema
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ModuleForm>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllModuleForms()
        {
            try
            {
                var moduleForms = await _moduleFormBusiness.GetAllModuleFormsAsync();
                return Ok(moduleForms);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener relaciones módulo-formulario");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener relaciones módulo-formulario");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Obtiene una relación específica entre módulo y formulario por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModuleForm), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetModuleFormById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID debe ser un número positivo." });
            }

            try
            {
                var moduleForm = await _moduleFormBusiness.GetModuleFormByIdAsync(id);
                return Ok(moduleForm);
            }
            catch (CustomValidationException ex)
            {
                _logger.LogWarning("Validación fallida para la relación módulo-formulario con ID: {ModuleFormId}. Error: {ErrorMessage}", id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning("Relación módulo-formulario no encontrada con ID: {ModuleFormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener relación módulo-formulario con ID: {ModuleFormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener relación módulo-formulario con ID: {ModuleFormId}", id);
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }

        /// <summary>
        /// Crea una nueva relación entre módulo y formulario en el sistema
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ModuleForm), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateModuleForm([FromBody] ModuleForm moduleForm)
        {
            if (moduleForm == null)
            {
                return BadRequest(new { message = "Los datos de la relación módulo-formulario son obligatorios." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Datos inválidos en la solicitud." });
            }

            try
            {
                var createdModuleForm = await _moduleFormBusiness.CreateModuleFormAsync(moduleForm);
                return CreatedAtAction(nameof(GetModuleFormById), new { id = createdModuleForm.Id }, createdModuleForm);
            }
            catch (CustomValidationException ex)
            {
                _logger.LogWarning("Validación fallida al crear relación módulo-formulario. Error: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear relación módulo-formulario");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear relación módulo-formulario");
                return StatusCode(500, new { message = "Ha ocurrido un error inesperado." });
            }
        }
    }
}
