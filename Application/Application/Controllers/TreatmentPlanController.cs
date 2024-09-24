using Microsoft.AspNetCore.Mvc;
using DentalBooking.ModelViews.TreatmentModels;
using DentalBooking_Contract_Services.Interface;
using System.Threading.Tasks;
using DentalBooking.Core.Base;
using DentalBooking.ModelViews.TreatmentPlanModels;

namespace DentalBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentPlanController : ControllerBase
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentPlanController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        // Lấy tất cả cuộc hẹn
        [HttpGet("appointments")]
        public async Task<IActionResult> GetAllAppointments()
        {
            try
            {
                var appointments = await _treatmentService.GetAllAppointmentsAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/TreatmentPlan/{customerId}
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetTreatmentPlan(int customerId)
        {
            try
            {
                var treatmentPlan = await _treatmentService.GetTreatmentPlanAsync(customerId);
                return Ok(treatmentPlan);
            }
            catch (KeyNotFoundException ex)
            {
                throw new BaseException.BadRequestException("not_found", ex.Message);
            }
            catch (Exception ex)
            {
                throw new BaseException.CoreException("server_error", ex.Message);
            }
        }

        // Gửi kế hoạch điều trị cho khách hàng
        [HttpPost("send-treatment-plan/{customerId}/{doctorId}")]
        public async Task<IActionResult> SendTreatmentPlan(int customerId, int doctorId)
        {
            try
            {
                await _treatmentService.SendTreatmentPlanToCustomerAsync(customerId, doctorId);
                return Ok("Kế hoạch điều trị đã được gửi thành công cho khách hàng.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Không tìm thấy khách hàng với ID: {customerId}. Chi tiết lỗi: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi khi gửi kế hoạch điều trị: {ex.Message}");
            }
        }

        // POST: api/TreatmentPlan
        [HttpPost]
        public async Task<IActionResult> CreateTreatmentPlan([FromBody] TreatmentPlanRequestModelView treatmentPlanRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new BaseException.BadRequestException("invalid_request", "Dữ liệu đầu vào không hợp lệ.");
            }

            try
            {
                var result = await _treatmentService.CreateTreatmentPlanAsync(treatmentPlanRequest);
                if (result)
                {
                    return CreatedAtAction(nameof(GetTreatmentPlan), new { customerId = treatmentPlanRequest.CustomerId }, treatmentPlanRequest);
                }
            }
            catch (Exception ex)
            {
                throw new BaseException.CoreException("server_error", ex.Message);
            }

            return BadRequest("Không thể tạo kế hoạch điều trị.");
        }

        // PUT: api/TreatmentPlan/{treatmentId}
        [HttpPut("{treatmentId}")]
        public async Task<IActionResult> UpdateTreatmentPlan(int treatmentId, [FromBody] TreatmentPlanRequestModelView treatmentPlanRequest)
        {
            if (!ModelState.IsValid)
            {
                throw new BaseException.BadRequestException("invalid_request", "Dữ liệu đầu vào không hợp lệ.");
            }

            try
            {
                var result = await _treatmentService.UpdateTreatmentPlanAsync(treatmentId, treatmentPlanRequest);
                if (result)
                {
                    return NoContent();
                }
            }
            catch (KeyNotFoundException ex)
            {
                throw new BaseException.BadRequestException("not_found", ex.Message);
            }
            catch (Exception ex)
            {
                throw new BaseException.CoreException("server_error", ex.Message);
            }

            return BadRequest("Không thể cập nhật kế hoạch điều trị.");
        }

        // DELETE: api/TreatmentPlan/{treatmentId}
        [HttpDelete("{treatmentId}")]
        public async Task<IActionResult> DeleteTreatmentPlan(int treatmentId)
        {
            try
            {
                var result = await _treatmentService.DeleteTreatmentPlanAsync(treatmentId);
                if (result)
                {
                    return NoContent();
                }
            }
            catch (KeyNotFoundException ex)
            {
                throw new BaseException.BadRequestException("not_found", ex.Message);
            }
            catch (Exception ex)
            {
                throw new BaseException.CoreException("server_error", ex.Message);
            }

            return BadRequest("Không thể xóa kế hoạch điều trị.");
        }
    }
}
