using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    /// <summary>
    /// Repositorio encargado de la gestión de la entidad RolUser en la base de datos.
    /// </summary>
    public class RolUserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolUserData> _logger;

        /// <summary>
        /// Constructor que recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Instancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>
        /// <param name="logger">Instancia de <see cref="ILogger"/> para el registro de errores.</param>
        public RolUserData(ApplicationDbContext context, ILogger<RolUserData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los registros de roles de usuarios almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de relaciones usuario-rol.</returns>
        public async Task<IEnumerable<RolUser>> GetAllAsync()
        {
            return await _context.Set<RolUser>().ToListAsync();
        }

        /// <summary>
        /// Obtiene un registro específico de usuario-rol por su identificador.
        /// </summary>
        /// <param name="id">Identificador único de la relación.</param>
        /// <returns>El registro encontrado o null si no existe.</returns>
        public async Task<RolUser?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<RolUser>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la relación usuario-rol con ID {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Crea una nueva relación usuario-rol en la base de datos.
        /// </summary>
        /// <param name="rolUser">Instancia de la relación usuario-rol a crear.</param>
        /// <returns>La relación creada.</returns>
        public async Task<RolUser> CreateAsync(RolUser rolUser)
        {
            try
            {
                await _context.Set<RolUser>().AddAsync(rolUser);
                await _context.SaveChangesAsync();
                return rolUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la relación usuario-rol");
                throw;
            }
        }

        /// <summary>
        /// Actualiza una relación usuario-rol existente en la base de datos.
        /// </summary>
        /// <param name="rolUser">Objeto con la información actualizada.</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> UpdateAsync(RolUser rolUser)
        {
            try
            {
                _context.Set<RolUser>().Update(rolUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la relación usuario-rol");
                return false;
            }
        }

        /// <summary>
        /// Elimina una relación usuario-rol de la base de datos.
        /// </summary>
        /// <param name="id">Identificador único de la relación a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var rolUser = await _context.Set<RolUser>().FindAsync(id);
                if (rolUser == null)
                    return false;

                _context.Set<RolUser>().Remove(rolUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la relación usuario-rol");
                return false;
            }
        }
    }
}
