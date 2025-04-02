using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class TypeInfractionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TypeInfractionData> _logger;

        public TypeInfractionData(ApplicationDbContext context, ILogger<TypeInfractionData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TypeInfraction>> GetAllAsync()
        {
            return await _context.Set<TypeInfraction>().ToListAsync();
        }

        public async Task<TypeInfraction?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<TypeInfraction>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la infracción con ID {Id}", id);
                throw;
            }
        }

        public async Task<TypeInfraction> CreateAsync(TypeInfraction infraction)
        {
            try
            {
                await _context.Set<TypeInfraction>().AddAsync(infraction);
                await _context.SaveChangesAsync();
                return infraction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la infracción: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(TypeInfraction infraction)
        {
            try
            {
                _context.Set<TypeInfraction>().Update(infraction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la infracción: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var infraction = await _context.Set<TypeInfraction>().FindAsync(id);
                if (infraction == null)
                    return false;

                _context.Set<TypeInfraction>().Remove(infraction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la infracción: {ex.Message}");
                return false;
            }
        }
    }
}
