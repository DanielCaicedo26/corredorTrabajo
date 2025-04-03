using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    /// <summary>
    /// Repositorio encargado de la gestión de la entidad InformationType en la base de datos.
    /// </summary>
    public class InformationTypeData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InformationTypeData> _logger;

        /// <summary>
        /// Constructor que recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Instancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>
        /// <param name="logger">Instancia de <see cref="ILogger{InformationTypeData}"/> para el registro de logs.</param>
        public InformationTypeData(ApplicationDbContext context, ILogger<InformationTypeData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los tipos de información almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de tipos de información.</returns>
        public async Task<IEnumerable<InformationType>> GetAllAsync()
        {
            try
            {
                return await _context.Set<InformationType>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los tipos de información");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un tipo de información específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador del tipo de información.</param>
        /// <returns>El tipo de información encontrado o null si no existe.</returns>
        public async Task<InformationType?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<InformationType>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tipo de información con ID {InformationTypeId}", id);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo tipo de información en la base de datos.
        /// </summary>
        /// <param name="informationType">Instancia del tipo de información a crear.</param>
        /// <returns>El tipo de información creado.</returns>
        public async Task<InformationType> CreateAsync(InformationType informationType)
        {
            try
            {
                await _context.Set<InformationType>().AddAsync(informationType);
                await _context.SaveChangesAsync();
                return informationType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el tipo de información");
                throw;
            }
        }

        /// <summary>
        /// Actualiza un tipo de información existente en la base de datos.
        /// </summary>
        /// <param name="informationType">Objeto con la información actualizada.</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> UpdateAsync(InformationType informationType)
        {
            try
            {
                _context.Set<InformationType>().Update(informationType);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el tipo de información");
                return false;
            }
        }

        /// <summary>
        /// Elimina un tipo de información de la base de datos.
        /// </summary>
        /// <param name="id">Identificador único del tipo de información a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un tipo de información con ID inválido: {InformationTypeId}", id);
                return false;
            }

            try
            {
                var informationType = await _context.Set<InformationType>().FindAsync(id);
                if (informationType == null)
                {
                    _logger.LogInformation("No se encontró ningún tipo de información con ID: {InformationTypeId}", id);
                    return false;
                }

                _context.Set<InformationType>().Remove(informationType);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el tipo de información con ID {InformationTypeId}", id);
                return false;
            }
        }
    }
}