using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class UserNotificationData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserNotificationData> _logger;

        public UserNotificationData(ApplicationDbContext context, ILogger<UserNotificationData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserNotification>> GetAllAsync()
        {
            return await _context.Set<UserNotification>().ToListAsync();
        }

        public async Task<UserNotification?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<UserNotification>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la notificación de usuario con ID {Id}", id);
                throw;
            }
        }

        public async Task<UserNotification> CreateAsync(UserNotification notification)
        {
            try
            {
                await _context.Set<UserNotification>().AddAsync(notification);
                await _context.SaveChangesAsync();
                return notification;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la notificación de usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UserNotification notification)
        {
            try
            {
                _context.Set<UserNotification>().Update(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la notificación de usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var notification = await _context.Set<UserNotification>().FindAsync(id);
                if (notification == null)
                    return false;

                _context.Set<UserNotification>().Remove(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la notificación de usuario: {ex.Message}");
                return false;
            }
        }
    }
}
