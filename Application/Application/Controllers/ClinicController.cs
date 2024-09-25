using DentalBooking.ModelViews.ClinicModelViews;
using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicService _clinicService;

        public ClinicController(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        // Đăng ký phòng khám
        [HttpPost]
        public async Task<IActionResult> RegisterClinic(ClinicRequestModelView model)
        {
            var createdClinic = await _clinicService.CreateClinicAsync(model);

            // Thêm thông báo thành công vào phản hồi
            return CreatedAtAction(nameof(GetClinic), new { id = createdClinic.Id }, new
            {
                Message = "Phòng khám đã được thêm thành công.",
                Clinic = createdClinic
            });
        }


        // Lấy thông tin phòng khám theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClinic(int id)
        {
            var clinic = await _clinicService.GetClinicByIdAsync(id);
            if (clinic == null)
            {
                return NotFound(new { Message = "Phòng khám không được tìm thấy." });
            }
            return Ok(clinic);
        }

        // Cập nhật thông tin phòng khám
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClinic(int id, ClinicRequestModelView model)
        {
            await _clinicService.UpdateClinicAsync(id, model);
            return Ok(new { Message = "Phòng khám đã được cập nhật thành công." });
        }

        // Xóa phòng khám
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinic(int id)
        {
            await _clinicService.DeleteClinicAsync(id);
            return Ok(new { Message = "Phòng khám đã được xóa thành công." });
        }

        // Lấy danh sách tất cả phòng khám
        [HttpGet]
        public async Task<IActionResult> GetAllClinics(int index = 1, int pageSize = 10)
        {
            var clinics = await _clinicService.GetClinicsAsync(index, pageSize);
            return Ok(clinics);
        }
    }
}
