using Entity.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class PaymentAgreementData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentAgreementData> _logger;

        public PaymentAgreementData(ApplicationDbContext context, ILogger<PaymentAgreementData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PaymentAgreement>> GetAllAsync()
        {
            return await _context.Set<PaymentAgreement>().ToListAsync();
        }

        public async Task<PaymentAgreement?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<PaymentAgreement>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el acuerdo de pago con ID {Id}", id);
                throw;
            }
        }

        public async Task<PaymentAgreement> CreateAsync(PaymentAgreement agreement)
        {
            try
            {
                await _context.Set<PaymentAgreement>().AddAsync(agreement);
                await _context.SaveChangesAsync();
                return agreement;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el acuerdo de pago: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(PaymentAgreement agreement)
        {
            try
            {
                _context.Set<PaymentAgreement>().Update(agreement);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el acuerdo de pago: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var agreement = await _context.Set<PaymentAgreement>().FindAsync(id);
                if (agreement == null)
                    return false;

                _context.Set<PaymentAgreement>().Remove(agreement);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el acuerdo de pago: {ex.Message}");
                return false;
            }
        }
    }
}
