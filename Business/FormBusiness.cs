using Data;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los formularios.
    /// </summary>
    public class FormBusiness
    {
        private readonly FormData _formData;
        private readonly ILogger<FormBusiness> _logger;

        public FormBusiness(FormData formData, ILogger<FormBusiness> logger)
        {
            _formData = formData;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los formularios en el sistema
        /// </summary>
        /// <returns>Lista de formularios como DTOs</returns>
        public async Task<IEnumerable<FormDto>> GetAllFormsAsync()
        {
            try
            {
                var forms = await _formData.GetAllAsync();
                return MapToDTOList(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
            }
        }

        /// <summary>
        /// Obtiene un formulario por su ID
        /// </summary>
        /// <param name="id">ID del formulario</param>
        /// <returns>Formulario encontrado</returns>
        /// <exception cref="ValidationException">Si el ID es inválido</exception>
        /// <exception cref="EntityNotFoundException">Si el formulario no existe</exception>
        public async Task<FormDto> GetFormByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un formulario con ID inválido: {FormId}", id);
                throw new ValidationException("id", "El ID del formulario debe ser mayor que cero");
            }

            try
            {
                var form = await _formData.GetByIdAsync(id);
                if (form == null)
                {
                    _logger.LogInformation("No se encontró ningún formulario con ID: {FormId}", id);
                    throw new EntityNotFoundException("Formulario", id);
                }

                return MapToDTO(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario con ID {id}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo formulario en el sistema
        /// </summary>
        /// <param name="formDto">Datos del formulario a crear</param>
        /// <returns>Formulario creado</returns>
        /// <exception cref="ValidationException">Si los datos del formulario son inválidos</exception>
        public async Task<FormDto> CreateFormAsync(FormDto formDto)
        {
            ValidateForm(formDto);

            try
            {
                var form = MapToEntity(formDto);
                var createdForm = await _formData.CreateAsync(form);
                return MapToDTO(createdForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo formulario: {FormName}", formDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
            }
        }

        /// <summary>
        /// Elimina un formulario por su ID
        /// </summary>
        /// <param name="id">ID del formulario a eliminar</param>
        /// <exception cref="ValidationException">Si el ID es inválido</exception>
        /// <exception cref="EntityNotFoundException">Si el formulario no existe</exception>
        public async Task DeleteFormAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un formulario con ID inválido: {FormId}", id);
                throw new ValidationException("id", "El ID del formulario debe ser mayor que cero");
            }

            try
            {
                var form = await _formData.GetByIdAsync(id);
                if (form == null)
                {
                    _logger.LogInformation("No se encontró ningún formulario para eliminar con ID: {FormId}", id);
                    throw new EntityNotFoundException("Formulario", id);
                }

                var deleted = await _formData.DeleteAsync(id);
                if (!deleted)
                {
                    throw new ExternalServiceException("Base de datos", "Error al eliminar el formulario");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el formulario con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el formulario con ID {id}", ex);
            }
        }

        /// <summary>
        /// Valida los datos del formulario antes de su creación o actualización
        /// </summary>
        /// <param name="formDto">Objeto FormDto a validar</param>
        /// <exception cref="ValidationException">Si los datos no son válidos</exception>
        private void ValidateForm(FormDto formDto)
        {
            if (formDto == null)
            {
                throw new ValidationException("El objeto formulario no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(formDto.Name))
            {
                _logger.LogWarning("Intento de crear un formulario con Name vacío");
                throw new ValidationException("Name", "El nombre del formulario es obligatorio");
            }
        }

        // Método para mapear de Form a FormDto
        private FormDto MapToDTO(Form form)
        {
            return new FormDto
            {
                Id = form.Id,
                Name = form.Name,
                Description = form.Description,
                DateCreation = form.DateCreation,
                Status = form.Status
            };
        }

        // Método para mapear de FormDto a Form
        private Form MapToEntity(FormDto formDto)
        {
            return new Form
            {
                Id = formDto.Id,
                Name = formDto.Name,
                Description = formDto.Description,
                DateCreation = formDto.DateCreation,
                Status = formDto.Status
            };
        }

        // Método para mapear una lista de Form a una lista de FormDto
        private IEnumerable<FormDto> MapToDTOList(IEnumerable<Form> forms)
        {
            var formsDto = new List<FormDto>();
            foreach (var form in forms)
            {
                formsDto.Add(MapToDTO(form));
            }
            return formsDto;
        }
    }
}


