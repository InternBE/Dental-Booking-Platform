using DentalBooking.ModelViews.ModelViews;
using DentalBooking_Contract_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class NotificationService : INotificationService
{
    private readonly List<NotificationModelView> _notifications = new List<NotificationModelView>();

    // Gửi thông báo tới khách hàng qua các phương tiện khác nhau
    public async Task SendNotificationAsync(int customerId, string message)
    {
        Console.WriteLine($"Gửi thông báo tới khách hàng {customerId}: {message}");

        // Tạo thông báo và lưu tạm trong bộ nhớ
        var notification = new NotificationModelView
        {
            NotificationId = _notifications.Count + 1, // Tăng ID tự động
            CustomerId = customerId,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        _notifications.Add(notification);

        // Mô phỏng tác vụ không đồng bộ
        await Task.CompletedTask;
    }

    // Gửi thông báo tới khách hàng và bác sĩ
    public async Task SendNotificationAsync(int customerId, int doctorId, string message)
    {
        Console.WriteLine($"Gửi thông báo tới khách hàng {customerId} từ bác sĩ {doctorId}: {message}");

        // Tạo thông báo và lưu tạm trong bộ nhớ
        var notification = new NotificationModelView
        {
            NotificationId = _notifications.Count + 1, // Tăng ID tự động
            CustomerId = customerId,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        _notifications.Add(notification);

        // Mô phỏng tác vụ không đồng bộ
        await Task.CompletedTask;
    }

    // Lấy danh sách thông báo của một khách hàng
    public async Task<IEnumerable<NotificationModelView>> GetNotificationsAsync(int customerId)
    {
        var customerNotifications = _notifications.Where(n => n.CustomerId == customerId);
        return await Task.FromResult(customerNotifications);
    }
}
