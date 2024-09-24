using Microsoft.AspNetCore.Mvc;
using DentalBooking.ModelViews.MessageModels;
using DentalBooking_Contract_Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Gửi tin nhắn
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequestModelView messageRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                var result = await _messageService.SendMessageAsync(messageRequest);
                if (result)
                {
                    return Ok("Tin nhắn đã được gửi thành công.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }

            return BadRequest("Gửi tin nhắn không thành công.");
        }

        // Lấy tất cả tin nhắn giữa hai người dùng
        [HttpGet("{senderId}/{receiverId}")]
        public async Task<IActionResult> GetMessages(int senderId, int receiverId)
        {
            try
            {
                var messages = await _messageService.GetMessagesAsync(senderId, receiverId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        // Đánh dấu tin nhắn là đã đọc
        [HttpPut("mark-as-read/{messageId}")]
        public async Task<IActionResult> MarkMessageAsRead(int messageId)
        {
            try
            {
                var result = await _messageService.MarkMessageAsReadAsync(messageId);
                if (result)
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }

            return BadRequest("Đánh dấu tin nhắn không thành công.");
        }
    }
}
