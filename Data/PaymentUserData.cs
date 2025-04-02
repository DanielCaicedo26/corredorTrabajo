using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class PaymentUserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentUserData> _logger;

        public PaymentUserData(ApplicationDbContext context, ILogger<PaymentUserData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PaymentUser>> GetAllAsync()
        {
            return await _context.Set<PaymentUser>().ToListAsync();
        }

        public async Task<PaymentUser?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<PaymentUser>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el PaymentUser con ID {Id}", id);
                throw;
            }
        }

        public async Task<PaymentUser> CreateAsync(PaymentUser paymentUser)
        {
            try
            {
                await _context.Set<PaymentUser>().AddAsync(paymentUser);
                await _context.SaveChangesAsync();
                return paymentUser;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el PaymentUser: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(PaymentUser paymentUser)
        {
            try
            {
                _context.Set<PaymentUser>().Update(paymentUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el PaymentUser: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var paymentUser = await _context.Set<PaymentUser>().FindAsync(id);
                if (paymentUser == null)
                    return false;

                _context.Set<PaymentUser>().Remove(paymentUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el PaymentUser: {ex.Message}");
                return false;
            }
        }
    }
}
