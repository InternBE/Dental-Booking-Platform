using DentalBooking.ModelViews.AppointmentModelViews;
using DentalBooking_Contract_Services.Interface;
using DentalBooking_Services.Service;
using Microsoft.AspNetCore.Mvc;


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

        // POST: api/Appointment
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

    }
}
