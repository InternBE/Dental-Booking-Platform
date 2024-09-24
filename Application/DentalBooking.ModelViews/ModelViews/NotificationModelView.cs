using System;

namespace DentalBooking.ModelViews.ModelViews
{
    public class NotificationModelView
    {
        public int NotificationId { get; set; } // ID thông báo
        public string Message { get; set; } = string.Empty; // Nội dung thông báo
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Ngày tạo
        public int CustomerId { get; set; } // ID khách hàng
        public bool IsRead { get; set; } = false; // Trạng thái đọc
    }
}
