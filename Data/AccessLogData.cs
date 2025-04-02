using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
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
        public async Task<IEnumerable<AccessLog>> GetAllAsync()
        {
            return await _context.Set<AccessLog>().ToListAsync();
        }

        /// <summary>
        /// Obtiene un registro de acceso por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del registro.</param>
        /// <returns>El registro encontrado o null si no existe.</returns>
        public async Task<AccessLog?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<AccessLog>().FindAsync(id);
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
        public async Task<AccessLog> CreateAsync(AccessLog accessLog)
        {
            try
            {
                await _context.Set<AccessLog>().AddAsync(accessLog);
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
        public async Task<bool> UpdateAsync(AccessLog accessLog)
        {
            try
            {
                _context.Set<AccessLog>().Update(accessLog);
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
                var accessLog = await _context.Set<AccessLog>().FindAsync(id);
                if (accessLog == null)
                    return false;

                _context.Set<AccessLog>().Remove(accessLog);
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
