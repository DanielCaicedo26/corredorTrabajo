using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class InformationInfractionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InformationInfractionData> _logger;

        public InformationInfractionData(ApplicationDbContext context, ILogger<InformationInfractionData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<InformationInfraction>> GetAllAsync()
        {
            return await _context.Set<InformationInfraction>().ToListAsync();
        }

        public async Task<InformationInfraction?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<InformationInfraction>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la infracción con ID {Id}", id);
                throw;
            }
        }

        public async Task<InformationInfraction> CreateAsync(InformationInfraction infraction)
        {
            try
            {
                await _context.Set<InformationInfraction>().AddAsync(infraction);
                await _context.SaveChangesAsync();
                return infraction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la infracción: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(InformationInfraction infraction)
        {
            try
            {
                _context.Set<InformationInfraction>().Update(infraction);
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
                var infraction = await _context.Set<InformationInfraction>().FindAsync(id);
                if (infraction == null)
                    return false;

                _context.Set<InformationInfraction>().Remove(infraction);
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
