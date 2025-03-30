using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace corredorTrabajo.Data
{
    public class ModuloFormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuloFormData> _logger;

        public ModuloFormData(ApplicationDbContext context, ILogger<ModuloFormData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ModuloForm>> GetAllAsync()
        {
            return await _context.Set<ModuloForm>().ToListAsync();
        }

        public async Task<ModuloForm?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<ModuloForm>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ModuloForm con ID {Id}", id);
                throw;
            }
        }

        public async Task<ModuloForm> CreateAsync(ModuloForm moduloForm)
        {
            try
            {
                await _context.Set<ModuloForm>().AddAsync(moduloForm);
                await _context.SaveChangesAsync();
                return moduloForm;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el ModuloForm: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ModuloForm moduloForm)
        {
            try
            {
                _context.Set<ModuloForm>().Update(moduloForm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el ModuloForm: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var moduloForm = await _context.Set<ModuloForm>().FindAsync(id);
                if (moduloForm == null)
                    return false;

                _context.Set<ModuloForm>().Remove(moduloForm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el ModuloForm: {ex.Message}");
                return false;
            }
        }
    }
}
