using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository;
using DentalBooking.Contract.Repository.IUOW;
using DentalBooking.ModelViews.MessageModels;
using DentalBooking_Contract_Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalBooking.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SendMessageAsync(MessageRequestModelView messageRequest)
        {
            // Tạo đối tượng Message từ request
            var message = new Message
            {
                Content = messageRequest.Content,
                SendDate = DateTime.UtcNow, // Ngày gửi tin nhắn
                SenderId = messageRequest.SenderId,
                ReceiverId = messageRequest.ReceiverId,
                CreatedTime = DateTime.UtcNow,
                LastUpdatedTime = DateTime.UtcNow
            };

            // Chèn vào cơ sở dữ liệu
            _unitOfWork.GetRepository<Message>().Insert(message);

            await _unitOfWork.SaveAsync();
            return true; // Nếu bạn muốn trả về true sau khi lưu thành công

        }


        public async Task<IEnumerable<MessageResponseModelView>> GetMessagesAsync(int senderId, int receiverId)
        {
            var messages = await _unitOfWork.GetRepository<Message>().GetAllAsync();
            return messages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                             (m.SenderId == receiverId && m.ReceiverId == senderId))
                .Select(m => new MessageResponseModelView
                {
                    Content = m.Content,
                    SendDate = m.SendDate,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId
                }).ToList();
        }

        public async Task<bool> MarkMessageAsReadAsync(int messageId)
        {
            var message = await _unitOfWork.GetRepository<Message>().GetByIdAsync(messageId);
            if (message != null)
            {
                _unitOfWork.GetRepository<Message>().Update(message);
                await _unitOfWork.SaveAsync();
                return true; // Trả về true nếu cập nhật thành công
            }
            return false; // Trả về false nếu không tìm thấy tin nhắn
        }
    }
}
