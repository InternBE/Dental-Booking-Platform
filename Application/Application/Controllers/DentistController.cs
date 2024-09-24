using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DentistController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService; // Khai báo

        // Inject IAppointmentService thông qua constructor
        public DentistController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService; // Gán giá trị inject vào biến toàn cục
        }
        [HttpGet("suggest-next-appointment")]
            public async Task<IActionResult> SuggestNextAppointment(int userId, int treatmentPlanId)
            {
                var nextAppointmentDate = await _appointmentService.SuggestNextAppointment(userId, treatmentPlanId);

                if (nextAppointmentDate == null)
                {
                    return NotFound("No valid next appointment available.");
                }

                return Ok(new { NextAppointmentDate = nextAppointmentDate });
            }

}
}
