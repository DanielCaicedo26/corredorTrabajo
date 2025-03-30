using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class UserInfractionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserInfractionData> _logger;

        public UserInfractionData(ApplicationDbContext context, ILogger<UserInfractionData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserInfraction>> GetAllAsync()
        {
            return await _context.Set<UserInfraction>().ToListAsync();
        }

        public async Task<UserInfraction?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<UserInfraction>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la infracción de usuario con ID {Id}", id);
                throw;
            }
        }

        public async Task<UserInfraction> CreateAsync(UserInfraction userInfraction)
        {
            try
            {
                await _context.Set<UserInfraction>().AddAsync(userInfraction);
                await _context.SaveChangesAsync();
                return userInfraction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la infracción de usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UserInfraction userInfraction)
        {
            try
            {
                _context.Set<UserInfraction>().Update(userInfraction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la infracción de usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var userInfraction = await _context.Set<UserInfraction>().FindAsync(id);
                if (userInfraction == null)
                    return false;

                _context.Set<UserInfraction>().Remove(userInfraction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la infracción de usuario: {ex.Message}");
                return false;
            }
        }
    }
}
