
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entity.Dto;
    using Entity.Model;
    using Microsoft.Extensions.Logging;
    using Utilities.Exceptions;

namespace Business
    {
        public class FormBusiness
        {
            private readonly FormData _formData;
            private readonly ILogger<FormBusiness> _logger;

            public FormBusiness(FormData formData, ILogger<FormBusiness> logger)
            {
                _formData = formData;
                _logger = logger;
            }

            public async Task<IEnumerable<FormDto>> GetAllFormsAsync()
            {
                try
                {
                    var forms = await _formData.GetAllAsync();
                    return forms.Select(MapToDto).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener todos los formularios");
                    throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
                }
            }

            public async Task<FormDto> GetFormByIdAsync(int id)
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó obtener un formulario con ID inválido: {FormId}", id);
                    throw new ArgumentException("El ID del formulario debe ser mayor que cero", nameof(id));
                }

                try
                {
                    var form = await _formData.GetByIdAsync(id);
                    if (form == null)
                    {
                        _logger.LogInformation("No se encontró ningún formulario con ID: {FormId}", id);
                        throw new InvalidOperationException($"No se encontró el formulario con ID {id}");
                    }

                    return MapToDto(form);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener el formulario con ID: {FormId}", id);
                    throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario con ID {id}", ex);
                }
            }

            public async Task<FormDto> CreateFormAsync(FormDto formDto)
            {
                try
                {
                    ValidateForm(formDto);
                    var form = MapToEntity(formDto);
                    var createdForm = await _formData.CreateAsync(form);
                    return MapToDto(createdForm);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear un nuevo formulario");
                    throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
                }
            }

            public async Task DeleteFormAsync(int id)
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Se intentó eliminar un formulario con ID inválido: {FormId}", id);
                    throw new ArgumentException("El ID del formulario debe ser mayor que cero", nameof(id));
                }

                try
                {
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

            private void ValidateForm(FormDto formDto)
            {
                if (formDto == null)
                {
                    throw new ArgumentNullException(nameof(formDto), "El objeto formulario no puede ser nulo");
                }

                if (string.IsNullOrWhiteSpace(formDto.Name))
                {
                    throw new ArgumentException("El nombre del formulario es obligatorio", nameof(formDto.Name));
                }

                if (string.IsNullOrWhiteSpace(formDto.Status))
                {
                    throw new ArgumentException("El estado del formulario es obligatorio", nameof(formDto.Status));
                }
            }
        
        private static FormDto MapToDto(Form form) => new FormDto
        {
            Id = form.Id,
            Name = form.Name,
            Description = form.Description,
            DateCreation = form.DateCreation,
            Status = form.Status
        };

        private static Form MapToEntity(FormDto formDto)
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
        }
    }


