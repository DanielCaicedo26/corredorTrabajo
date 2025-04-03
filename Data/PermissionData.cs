using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{

    public class PermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionData> _logger;

        public PermissionData(ApplicationDbContext context, ILogger<PermissionData> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos almacenados en la base de datos.
        /// </summary>
        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Set<Permission>().ToListAsync();
        }

        /// <summary>
        /// Obtiene un permiso por su identificador.
        /// </summary>
        public async Task<Permission?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Permission>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo permiso en la base de datos.
        /// </summary>
        public async Task<Permission> CreateAsync(Permission permission)
        {
            try
            {
                await _context.Set<Permission>().AddAsync(permission);
                await _context.SaveChangesAsync();
                return permission;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el permiso: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza la información de un permiso existente.
        /// </summary>
        public async Task<bool> UpdateAsync(Permission permission)
        {
            try
            {
                _context.Set<Permission>().Update(permission);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el permiso: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Elimina un permiso por su identificador.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<Permission>().FindAsync(id);
                if (entity == null)
                    return false;

                _context.Set<Permission>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el permiso: {ex.Message}");
                return false;
            }
        }
    }
}

