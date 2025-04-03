using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class PaymentHistoryData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentHistoryData> _logger;

        public PaymentHistoryData(ApplicationDbContext context, ILogger<PaymentHistoryData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PaymentHistory>> GetAllAsync()
        {
            return await _context.Set<PaymentHistory>().ToListAsync();
        }

        public async Task<PaymentHistory?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<PaymentHistory>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de pago con ID {Id}", id);
                throw;
            }
        }

        public async Task<PaymentHistory> CreateAsync(PaymentHistory paymentHistory)
        {
            try
            {
                await _context.Set<PaymentHistory>().AddAsync(paymentHistory);
                await _context.SaveChangesAsync();
                return paymentHistory;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el historial de pago: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(PaymentHistory paymentHistory)
        {
            try
            {
                _context.Set<PaymentHistory>().Update(paymentHistory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el historial de pago: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var paymentHistory = await _context.Set<PaymentHistory>().FindAsync(id);
                if (paymentHistory == null)
                    return false;

                _context.Set<PaymentHistory>().Remove(paymentHistory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el historial de pago: {ex.Message}");
                return false;
            }
        }
    }
}
