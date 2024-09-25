using DentalBooking.ModelViews.DentistModelViews;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<List<DentistResponseModelView>>> GetAllAppointments()
        {
            var appointments = await _dentistService.GetAllAppointmentsAsync();
            if (appointments == null || !appointments.Any())
            {
                return NotFound(new { Message = "Không tìm thấy lịch hẹn nào." });
            }

            var response = appointments.Select(a => new DentistResponseModelView
            {
                Id = a.Id,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                UserId = a.UserId,
                ClinicId = a.ClinicId,
                TreatmentPlanId = a.TreatmentPlanId
            }).ToList();

            return Ok(response);
        }

        // Lấy lịch hẹn theo ID
        [HttpGet("appointments/{id}")]
        public async Task<ActionResult<DentistResponseModelView>> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await _dentistService.GetAppointmentByIdAsync(id);
                var response = new DentistResponseModelView
                {
                    Id = appointment.Id,
                    AppointmentDate = appointment.AppointmentDate,
                    Status = appointment.Status,
                    UserId = appointment.UserId,
                    ClinicId = appointment.ClinicId,
                    TreatmentPlanId = appointment.TreatmentPlanId
                };

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = $"Không tìm thấy lịch hẹn với ID {id}: {ex.Message}" });
            }
        }

        // Tạo mới lịch hẹn
        [HttpPost("appointments")]
        public async Task<ActionResult<DentistResponseModelView>> ScheduleAppointment(DentistRequestModelView request)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Thông tin lịch hẹn là bắt buộc." });
            }

            var appointment = new Appointment
            {
                AppointmentDate = request.AppointmentDate,
                UserId = request.UserId,
                ClinicId = request.ClinicId,
                TreatmentPlanId = request.TreatmentPlanId,
                Status = request.Status
            };

            var createdAppointment = await _dentistService.ScheduleAppointmentAsync(appointment);
            var response = new DentistResponseModelView
            {
                Id = createdAppointment.Id,
                AppointmentDate = createdAppointment.AppointmentDate,
                Status = createdAppointment.Status,
                UserId = createdAppointment.UserId,
                ClinicId = createdAppointment.ClinicId,
                TreatmentPlanId = createdAppointment.TreatmentPlanId
            };

            return CreatedAtAction(nameof(GetAppointmentById), new { id = response.Id }, response);
        }

        // Cập nhật lịch hẹn
        [HttpPut("appointments/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, DentistRequestModelView request)
        {
            if (request == null || id != request.UserId) // Kiểm tra request và ID
            {
                return BadRequest(new { Message = "Thông tin lịch hẹn không hợp lệ." });
            }

            var appointment = new Appointment
            {
                Id = id,
                AppointmentDate = request.AppointmentDate,
                Status = request.Status,
                UserId = request.UserId,
                ClinicId = request.ClinicId,
                TreatmentPlanId = request.TreatmentPlanId
            };

            try
            {
                await _dentistService.UpdateAppointmentAsync(appointment);
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
                await _dentistService.DeleteAppointmentAsync(id);
                return Ok(new { Message = $"Lịch hẹn với ID {id} đã được xóa thành công." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = $"Không tìm thấy lịch hẹn với ID {id}: {ex.Message}" });
            }
        }

        // Lên lịch tái khám
        [HttpPost("appointments/{id}/followup")]
        public async Task<ActionResult<DentistResponseModelView>> ScheduleFollowUp(int id)
        {
            try
            {
                var followUpAppointment = await _dentistService.ScheduleFollowUpAppointmentAsync(id);
                var response = new DentistResponseModelView
                {
                    Id = followUpAppointment.Id,
                    AppointmentDate = followUpAppointment.AppointmentDate,
                    Status = followUpAppointment.Status,
                    UserId = followUpAppointment.UserId,
                    ClinicId = followUpAppointment.ClinicId,
                    TreatmentPlanId = followUpAppointment.TreatmentPlanId
                };

                return CreatedAtAction(nameof(GetAppointmentById), new { id = response.Id }, response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = $"Không tìm thấy lịch hẹn với ID {id}: {ex.Message}" });
            }
        }
    }
}
