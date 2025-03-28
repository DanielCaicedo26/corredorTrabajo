using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace corredorTrabajo.Data
{
    /// <summary>
    /// Repositorio encargado de la gestión de los registros de acceso en la base de datos.
    /// </summary>
    public class AccessLogData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccessLogData> _logger;

        /// <summary>
        /// Constructor que recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Instancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>
        /// <param name="logger">Instancia de <see cref="ILogger"/> para el registro de errores.</param>
        public AccessLogData(ApplicationDbContext context, ILogger<AccessLogData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los registros de acceso almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de registros de acceso.</returns>
        public async Task<IEnumerable<AccessLogDto>> GetAllAsync()
        {
            return await _context.Set<AccessLogDto>().ToListAsync();
        }

        /// <summary>
        /// Obtiene un registro de acceso por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del registro.</param>
        /// <returns>El registro encontrado o null si no existe.</returns>
        public async Task<AccessLogDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<AccessLogDto>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el registro de acceso con ID {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo registro de acceso en la base de datos.
        /// </summary>
        /// <param name="accessLog">Instancia del registro a crear.</param>
        /// <returns>El registro creado.</returns>
        public async Task<AccessLogDto> CreateAsync(AccessLogDto accessLog)
        {
            try
            {
                await _context.Set<AccessLogDto>().AddAsync(accessLog);
                await _context.SaveChangesAsync();
                return accessLog;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el registro de acceso");
                throw;
            }
        }

        /// <summary>
        /// Actualiza un registro de acceso existente en la base de datos.
        /// </summary>
        /// <param name="accessLog">Objeto con la información actualizada.</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> UpdateAsync(AccessLogDto accessLog)
        {
            try
            {
                _context.Set<AccessLogDto>().Update(accessLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el registro de acceso");
                return false;
            }
        }

        /// <summary>
        /// Elimina un registro de acceso de la base de datos.
        /// </summary>
        /// <param name="id">Identificador único del registro a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var accessLog = await _context.Set<AccessLogDto>().FindAsync(id);
                if (accessLog == null)
                    return false;

                _context.Set<AccessLogDto>().Remove(accessLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el registro de acceso");
                return false;
            }
        }
    }
}
