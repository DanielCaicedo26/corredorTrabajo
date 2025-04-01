using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class RolFormPermissionData
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RolFormPermissionData> _logger;

    public RolFormPermissionData(ApplicationDbContext context, ILogger<RolFormPermissionData> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las relaciones entre roles, formularios y permisos.
    /// </summary>
    public async Task<IEnumerable<RolFormPermission>> GetAllAsync()
    {
        return await _context.Set<RolFormPermission>().ToListAsync();
    }

    /// <summary>
    /// Obtiene una relación específica por su identificador.
    /// </summary>
    public async Task<RolFormPermission?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Set<RolFormPermission>().FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la relación con ID {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// Crea una nueva relación entre rol, formulario y permiso.
    /// </summary>
    public async Task<RolFormPermission> CreateAsync(RolFormPermission rolFormPermission)
    {
        try
        {
            await _context.Set<RolFormPermission>().AddAsync(rolFormPermission);
            await _context.SaveChangesAsync();
            return rolFormPermission;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al crear la relación: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Actualiza una relación existente.
    /// </summary>
    public async Task<bool> UpdateAsync(RolFormPermission rolFormPermission)
    {
        try
        {
            _context.Set<RolFormPermission>().Update(rolFormPermission);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al actualizar la relación: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Elimina una relación por su identificador.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await _context.Set<RolFormPermission>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<RolFormPermission>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al eliminar la relación: {ex.Message}");
            return false;
        }
    }
}
}

