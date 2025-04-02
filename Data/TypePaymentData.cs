using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class TypePaymentData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TypePaymentData> _logger;

        public TypePaymentData(ApplicationDbContext context, ILogger<TypePaymentData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TypePayment>> GetAllAsync()
        {
            return await _context.Set<TypePayment>().ToListAsync();
        }

        public async Task<TypePayment?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<TypePayment>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el tipo de pago con ID {Id}", id);
                throw;
            }
        }

        public async Task<TypePayment> CreateAsync(TypePayment typePayment)
        {
            try
            {
                await _context.Set<TypePayment>().AddAsync(typePayment);
                await _context.SaveChangesAsync();
                return typePayment;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el tipo de pago: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(TypePayment typePayment)
        {
            try
            {
                _context.Set<TypePayment>().Update(typePayment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el tipo de pago: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var typePayment = await _context.Set<TypePayment>().FindAsync(id);
                if (typePayment == null)
                    return false;

                _context.Set<TypePayment>().Remove(typePayment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el tipo de pago: {ex.Message}");
                return false;
            }
        }
    }
}
