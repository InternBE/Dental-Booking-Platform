using DentalBooking.Core.Base;
using DentalBooking.ModelViews.ClinicModelViews;
using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> RegisterClinic(ClinicModelView model)
        {
            var createdClinic = await _clinicService.CreateClinicAsync(model);
            return CreatedAtAction(nameof(GetClinic), new { id = createdClinic.Id }, createdClinic);
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClinic(int id, ClinicModelView model)
        {
            // Chỉ cần gọi phương thức mà không cần gán kết quả
            var clinicExists = await _clinicService.GetClinicByIdAsync(id);
            if (clinicExists == null)
            {
                return NotFound(new { Message = "Phòng khám không tồn tại." });
            }

            await _clinicService.UpdateClinicAsync(id, model);
            return Ok(new { Message = "Phòng khám đã được cập nhật thành công." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinic(int id)
        {
            // Tương tự, không cần gán kết quả nếu phương thức trả về void
            var clinicExists = await _clinicService.GetClinicByIdAsync(id);
            if (clinicExists == null)
            {
                return NotFound(new { Message = "Phòng khám không tồn tại." });
            }

            await _clinicService.DeleteClinicAsync(id);
            return Ok(new { Message = "Phòng khám đã được xóa thành công." });
        }


        [HttpGet]
        public async Task<IActionResult> GetAllClinics(int index = 1, int pageSize = 10)
        {
            var clinics = await _clinicService.GetClinicsAsync(index, pageSize);
            return Ok(clinics);
        }

    }
}
