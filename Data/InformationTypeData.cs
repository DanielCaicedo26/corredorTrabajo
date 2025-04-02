using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class InformationTypeInfractionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InformationTypeInfractionData> _logger;

        public InformationTypeInfractionData(ApplicationDbContext context, ILogger<InformationTypeInfractionData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<InformationTypeInfraction>> GetAllAsync()
        {
            return await _context.Set<InformationTypeInfraction>().ToListAsync();
        }

        public async Task<InformationTypeInfraction?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<InformationTypeInfraction>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la infracción de tipo con ID {Id}", id);
                throw;
            }
        }

        public async Task<InformationTypeInfraction> CreateAsync(InformationTypeInfraction infraction)
        {
            try
            {
                await _context.Set<InformationTypeInfraction>().AddAsync(infraction);
                await _context.SaveChangesAsync();
                return infraction;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la infracción de tipo: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(InformationTypeInfraction infraction)
        {
            try
            {
                _context.Set<InformationTypeInfraction>().Update(infraction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la infracción de tipo: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var infraction = await _context.Set<InformationTypeInfraction>().FindAsync(id);
                if (infraction == null)
                    return false;

                _context.Set<InformationTypeInfraction>().Remove(infraction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la infracción de tipo: {ex.Message}");
                return false;
            }
        }

        public  Task<InformationTypeInfraction> CreateAsync(InformationType infoType)
        {
            throw new NotImplementedException();
        }

        public  Task<bool> UpdateAsync(InformationType infoType)
        {
            throw new NotImplementedException();
        }
      }
}



