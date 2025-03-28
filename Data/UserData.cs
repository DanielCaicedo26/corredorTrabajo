using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace corredorTrabajo.Data
{


public class UserData
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserData> _logger;

    public UserData(ApplicationDbContext context, ILogger<UserData> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los usuarios almacenados en la base de datos.
    /// </summary>
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Set<User>().ToListAsync();
    }

    /// <summary>
    /// Obtiene un usuario por su identificador.
    /// </summary>
    public async Task<User?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Set<User>().FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el usuario con ID {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// Crea un nuevo usuario en la base de datos.
    /// </summary>
    public async Task<User> CreateAsync(User user)
    {
        try
        {
            user.RegistrationDate = DateTime.UtcNow; // Asigna la fecha actual
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al crear el usuario: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Actualiza la información de un usuario existente.
    /// </summary>
    public async Task<bool> UpdateAsync(User user)
    {
        try
        {
            _context.Set<User>().Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Elimina un usuario por su identificador.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await _context.Set<User>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<User>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al eliminar el usuario: {ex.Message}");
            return false;
        }
    }
}
