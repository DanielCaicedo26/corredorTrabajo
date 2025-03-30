using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace corredorTrabajo.Data
{
    public class UserViolationData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserViolationData> _logger;

        public UserViolationData(ApplicationDbContext context, ILogger<UserViolationData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserViolation>> GetAllAsync()
        {
            return await _context.Set<UserViolation>().ToListAsync();
        }

        public async Task<UserViolation?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<UserViolation>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la infracción del usuario con ID {Id}", id);
                throw;
            }
        }

        public async Task<UserViolation> CreateAsync(UserViolation violation)
        {
            try
            {
                await _context.Set<UserViolation>().AddAsync(violation);
                await _context.SaveChangesAsync();
                return violation;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la infracción del usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UserViolation violation)
        {
            try
            {
                _context.Set<UserViolation>().Update(violation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la infracción del usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var violation = await _context.Set<UserViolation>().FindAsync(id);
                if (violation == null)
                    return false;

                _context.Set<UserViolation>().Remove(violation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la infracción del usuario: {ex.Message}");
                return false;
            }
        }
    }
}
