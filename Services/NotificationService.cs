using Microsoft.EntityFrameworkCore;
using NewTalent.Data;
using NewTalent.Data.Entities;

namespace NewTalent.Services;

/// <summary>
/// Service for managing user notifications
/// </summary>
public class NotificationService
{
    private readonly NewTalentDbContext _context;

    public NotificationService(NewTalentDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create a new notification for a user
    /// </summary>
    public async Task CreateNotificationAsync(int userId, string title, string message, string type = "info", string? link = null)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            Link = link,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get unread notifications for a user
    /// </summary>
    public async Task<List<Notification>> GetUnreadNotificationsAsync(int userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Get all notifications for a user
    /// </summary>
    public async Task<List<Notification>> GetAllNotificationsAsync(int userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Mark notification as read
    /// </summary>
    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Mark all notifications as read for a user
    /// </summary>
    public async Task MarkAllAsReadAsync(int userId)
    {
        var unreadNotifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get unread count for a user
    /// </summary>
    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }

    /// <summary>
    /// Delete a notification
    /// </summary>
    public async Task DeleteNotificationAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}
