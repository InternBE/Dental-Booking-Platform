using DentalBooking.ModelViews.MessageModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalBooking_Contract_Services.Interface
{
    public interface IMessageService
    {
        // Gửi tin nhắn
        Task<bool> SendMessageAsync(MessageRequestModelView messageRequest);

        // Lấy tất cả tin nhắn giữa hai người dùng
        Task<IEnumerable<MessageResponseModelView>> GetMessagesAsync(int senderId, int receiverId);

        // Đánh dấu tin nhắn là đã đọc
        Task<bool> MarkMessageAsReadAsync(int messageId);
    }
}
