using System.ComponentModel.DataAnnotations;

namespace DentalBooking.ModelViews.MessageModels
{
    public class MessageRequestModelView
    {
        [Required]
        public int SenderId { get; set; } // Thêm SenderId
        [Required]
        public int ReceiverId { get; set; } // ID người nhận
        [Required]
        [StringLength(500, ErrorMessage = "Nội dung tin nhắn không được vượt quá 500 ký tự.")]
        public string Content { get; set; } = string.Empty; // Nội dung tin nhắn
    }
}
