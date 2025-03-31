using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class PaymentData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentData> _logger;

        public PaymentData(ApplicationDbContext context, ILogger<PaymentData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Set<Payment>().ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Payment>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el pago con ID {Id}", id);
                throw;
            }
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            try
            {
                await _context.Set<Payment>().AddAsync(payment);
                await _context.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el pago: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Payment payment)
        {
            try
            {
                _context.Set<Payment>().Update(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el pago: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var payment = await _context.Set<Payment>().FindAsync(id);
                if (payment == null)
                    return false;

                _context.Set<Payment>().Remove(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el pago: {ex.Message}");
                return false;
            }
        }
    }
}
