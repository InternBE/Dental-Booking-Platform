using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IClinicService _clinicService;
        private readonly IUserService _userService;

        public AdminController(IClinicService clinicService, IUserService userService)
        {
            _clinicService = clinicService;
            _userService = userService;
        }

        // Xét duyệt phòng khám
        [HttpPut("approve-clinic/{clinicId}")]
        public async Task<ActionResult> ApproveClinic(int clinicId)
        {
            var result = await _clinicService.ApproveClinicAsync(clinicId);
            if (!result)
            {
                return NotFound(new { Message = "Phòng khám không được tìm thấy." });
            }

            return Ok(new { Message = "Phòng khám đã được xét duyệt thành công." });
        }

        // Xét duyệt bác sĩ
        [HttpPut("approve-doctor/{doctorId}")]
        public async Task<ActionResult> ApproveDoctor(string doctorId)
        {
            var result = await _userService.ApproveDoctorAsync(doctorId);
            if (!result)
            {
                return NotFound(new { Message = "Bác sĩ không được tìm thấy." });
            }

            return Ok(new { Message = "Bác sĩ đã được xét duyệt thành công." });
        }
    }
}
