using Azure;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.ModelViews.AppointmentModelViews;
using DentalBooking_Contract_Services.Interface;
using DentalBooking_Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using DentalBooking.Contract.Repository.IUOW;
using DentalBooking.Core.Base;

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

        // GET: api/Appointment/all
        [HttpGet("all")]
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

        // GET: api/Appointment
        [HttpGet]
        public async Task<IActionResult> GetPaginatedAppointments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new { message = "Page number and page size must be greater than 0." });
            }

            var appointments = await _appointmentServices.GetPaginatedAppointmentsAsync(pageNumber, pageSize);
            var totalRecords = await _appointmentServices.GetTotalAppointmentsCountAsync();

            var response = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = appointments.Select(appointment => new AppointmentRequestModelView
                {
                    AppointmentDate = appointment.AppointmentDate,
                    Status = appointment.Status,
                    UserId = appointment.UserId,
                    ClinicId = appointment.ClinicId,
                    TreatmentPlanId = appointment.TreatmentPlanId
                })
            };

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

        // GET: api/Appointment/AllAppointmentByUserId
        [HttpGet("AllAppointmentByUserId")]
        public async Task<IActionResult> AllAppointmentByUserId([FromQuery] int userId)
        {
            var response = await _appointmentServices.AllAppointmentsByUserIdAsync(userId);
            if (response.IsNullOrEmpty())
            {
                return NotFound(new { message = "No appointments found for the user." });
            }

            return Ok(response);
        }

        // GET: api/Appointment/AlertDayAfter
        [HttpGet("AlertDayAfter")]
        public async Task<IActionResult> Alert([FromQuery] int userId, [FromQuery] bool isAlert = true)
        {
            var appointmentDayAfter = await _appointmentServices.AlertAppointmentDayAfter(userId, isAlert);
            if (appointmentDayAfter == null)
            {
                return NotFound(new { message = "No appointments found for tomorrow." });
            }
            return Ok(appointmentDayAfter);
        }

        // POST: api/Appointment/Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentRequestModelView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAppointment = await _appointmentServices.CreateAppointmentAsync(model);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.AppointmentDate }, createdAppointment);
        }

        // PUT: api/Appointment/Update/{id}
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

        // DELETE: api/Appointment/Delete/{id}
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

        // POST: api/Appointment/BookOneTime
        [HttpPost("BookOneTime")]
        public async Task<IActionResult> BookOneTimeAppointment([FromBody] AppointmentRequestModelView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAppointment = await _appointmentServices.BookOneTimeAppointmentAsync(model);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.AppointmentDate }, createdAppointment);
        }

        // POST: api/Appointment/BookPeriodic
        [HttpPost("BookPeriodic")]
        public async Task<IActionResult> BookPeriodicAppointments([FromBody] AppointmentRequestModelView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _appointmentServices.BookPeriodicAppointmentsAsync(model, 12);
            return Ok(response);
        }

        // GET: api/Appointment/dentist-weekly-schedule
        [Authorize(Roles = "Dentist")]
        [HttpGet("dentist-weekly-schedule")]
        public async Task<IActionResult> GetWeeklyScheduleForDentist()
        {
            try
            {
                var dentistIdClaim = User.Claims.FirstOrDefault(c => c.Type == "DentistId")?.Value;
                if (dentistIdClaim == null)
                {
                    return Unauthorized(new BaseResponse<string>
                    {
                        Success = false,
                        Message = "Dentist ID not found in token."
                    });
                }

                if (!int.TryParse(dentistIdClaim, out var dentistId))
                {
                    return BadRequest(new BaseResponse<string>
                    {
                        Success = false,
                        Message = "Invalid Dentist ID."
                    });
                }

                List<AppointmentResponeModelViews> appointments = await _appointmentServices.GetWeeklyScheduleForDentist(dentistId);
                if (appointments == null || !appointments.Any())
                {
                    return NotFound(new BaseResponse<string>
                    {
                        Success = false,
                        Message = "No appointments found for the dentist this week."
                    });
                }

                return Ok(new BaseResponse<List<AppointmentResponeModelViews>>
                {
                    Success = true,
                    Data = appointments,
                    Message = "Successfully retrieved the weekly schedule."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse<string>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }
    }
}
