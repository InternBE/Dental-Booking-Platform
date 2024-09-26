using Azure;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.ModelViews.AppointmentModelViews;
using DentalBooking_Contract_Services.Interface;
using DentalBooking_Services.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentServices;

        public AppointmentController(IAppointmentService appointmentServices)
        {
            _appointmentServices = appointmentServices;
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentServices.GetAllAppointmentsAsync();
            var response = appointments.Select(appointment => new AppointmentRequestModelView
            {
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                UserId = appointment.UserId,
                ClinicId = appointment.ClinicId,
                TreatmentPlanId = appointment.TreatmentPlanId
            });

            return Ok(response);
        }

        // GET: api/Appointment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentServices.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var response = new AppointmentRequestModelView
            {
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                UserId = appointment.UserId,
                ClinicId = appointment.ClinicId,
                TreatmentPlanId = appointment.TreatmentPlanId
            };

            return Ok(response);
        }
        // GET: api/Appointment/{id}
        [HttpGet("AllAppointmentByUserId")]
        public async Task<IActionResult> AllAppointmentByUserId([FromQuery] int UserId)
        {
            var response = await _appointmentServices.AllAppointmentsByUserIdAsync(UserId);
            if (response.IsNullOrEmpty())
            {
                return NotFound(new { message = "Dont have appointment" });
            }

            return Ok(response);
        }
        [HttpGet("AlertDayAfter")]
        public async Task<IActionResult> Alert([FromQuery] int UserId, [FromQuery] bool isAlert = true)
        {
            IEnumerable<AppointmentResponeModelViews> AppointmenDayAfter = await _appointmentServices.AlertAppointmentDayBefore(UserId, isAlert);
            if (AppointmenDayAfter == null)
            {
                return NotFound(new { message = "Dont have Appointment Tomorrow" });
            }
            return Ok(AppointmenDayAfter);
        }
        // POST: api/Appointment
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentRequestModelView model)
        {
            try
            {
                var result = await _appointmentServices.CreateAppointmentAsync(model);
                return Ok(new
                {
                    message = "Đặt lịch hẹn thành công!",
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra trong quá trình đặt lịch." });
            }
        }

        // PUT: api/Appointment/{id}
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentRequestModelView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _appointmentServices.UpdateAppointmentAsync(id, model);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Appointment/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var result = await _appointmentServices.DeleteAppointmentAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        // POST: api/Appointment/BookPeriodic
        [HttpPost("BookPeriodic")]
        public async Task<IActionResult> BookPeriodicAppointments([FromBody] AppointmentRequestModelView model, [FromQuery] int months = 12)
        {
            // Kiểm tra model có hợp lệ hay không
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra số tháng có hợp lệ không (ví dụ: tối đa 12 tháng)
            if (months <= 0 || months > 12)
            {
                return BadRequest("Số tháng phải nằm trong khoảng từ 1 đến 12.");
            }

            try
            {
                // Gọi hàm service để đăng ký lịch định kỳ
                var response = await _appointmentServices.BookPeriodicAppointmentsAsync(model, months);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                // Xử lý lỗi nếu có trùng lịch
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                return StatusCode(500, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

    }
}
