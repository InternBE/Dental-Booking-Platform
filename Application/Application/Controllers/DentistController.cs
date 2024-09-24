using DentalBooking.Contract.Repository.Entity;
using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DentistController : ControllerBase
    {
        private readonly IDentistService _dentistService;

        public DentistController(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        // Lấy tất cả lịch hẹn
        [HttpGet("appointments")]
        public async Task<ActionResult<List<Appointment>>> GetAllAppointments()
        {
            var appointments = await _dentistService.GetAllAppointmentsAsync();
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound(new { Message = "Không tìm thấy lịch hẹn nào." });
            }
            return Ok(appointments);
        }

        // Lấy lịch hẹn theo ID
        [HttpGet("appointments/{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await _dentistService.GetAppointmentByIdAsync(id);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = $"Không tìm thấy lịch hẹn với ID {id}: {ex.Message}" });
            }
        }

        // Tạo mới lịch hẹn
        [HttpPost("appointments")]
        public async Task<ActionResult<Appointment>> ScheduleAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest(new { Message = "Thông tin lịch hẹn là bắt buộc." });
            }

            var createdAppointment = await _dentistService.ScheduleAppointmentAsync(appointment);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.Id }, createdAppointment);
        }

        // Cập nhật lịch hẹn
        [HttpPut("appointments/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest(new { Message = "ID lịch hẹn không khớp." });
            }

            try
            {
                var result = await _dentistService.UpdateAppointmentAsync(appointment);
                return Ok(new { Message = $"Lịch hẹn với ID {id} đã được cập nhật thành công." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = $"Không tìm thấy lịch hẹn với ID {id}: {ex.Message}" });
            }
        }

        // Xóa lịch hẹn
        [HttpDelete("appointments/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var result = await _dentistService.DeleteAppointmentAsync(id);
                return Ok(new { Message = $"Lịch hẹn với ID {id} đã được xóa thành công." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = $"Không tìm thấy lịch hẹn với ID {id}: {ex.Message}" });
            }
        }
    }
}
