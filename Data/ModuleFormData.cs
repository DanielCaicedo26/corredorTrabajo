using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class ModuleFormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ModuleFormData> _logger;

        public ModuleFormData(ApplicationDbContext context, ILogger<ModuleFormData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ModuleForm>> GetAllAsync()
        {
            return await _context.Set<ModuleForm>().ToListAsync();
        }

        public async Task<ModuleForm?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<ModuleForm>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ModuloForm con ID {Id}", id);
                throw;
            }
        }

        public async Task<ModuleForm> CreateAsync(ModuleForm moduloForm)
        {
            try
            {
                await _context.Set<ModuleForm>().AddAsync(moduloForm);
                await _context.SaveChangesAsync();
                return moduloForm;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el ModuloForm: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ModuleForm moduloForm)
        {
            try
            {
                _context.Set<ModuleForm>().Update(moduloForm);
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
                var moduloForm = await _context.Set<ModuleForm>().FindAsync(id);
                if (moduloForm == null)
                    return false;

                _context.Set<ModuleForm>().Remove(moduloForm);
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
