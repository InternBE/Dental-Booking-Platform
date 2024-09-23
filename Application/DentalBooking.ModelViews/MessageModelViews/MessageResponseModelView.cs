using System;

namespace DentalBooking.ModelViews.MessageModels
{
    public class MessageResponseModelView
    {
        public int MessageId { get; set; } // ID tin nhắn
        public int SenderId { get; set; } // ID người gửi
        public int ReceiverId { get; set; } // ID người nhận
        public string Content { get; set; } = string.Empty; // Nội dung tin nhắn
        public DateTime SendDate { get; set; } // Thời gian gửi
        public bool IsRead { get; set; } // Trạng thái đã đọc
    }
}
