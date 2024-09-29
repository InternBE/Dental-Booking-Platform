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
        private readonly ITreatmentPlanService _treatmentService;

        public TreatmentPlanController(ITreatmentPlanService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        // Lấy tất cả cuộc hẹn với phân trang
        [HttpGet]
        public async Task<IActionResult> GetPaginatedTreatmentPlans([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest(new { message = "Page number and page size must be greater than 0." });
                }

                var treatmentPlans = await _treatmentService.GetPaginatedTreatmentPlansAsync(pageNumber, pageSize);
                var totalRecords = await _treatmentService.GetTotalTreatmentPlansCountAsync();

                var response = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                    Data = treatmentPlans
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Code = "server_error", Message = ex.Message });
            }
        }

        // GET: api/TreatmentPlan/{customerId}
        [HttpGet("GetAll/{customerId}")]
        public async Task<IActionResult> GetTreatmentPlan(int customerId)
        {
            try
            {
                var treatmentPlans = await _treatmentService.GetAllTreatmentPlansForCustomerAsync(customerId);

                if (treatmentPlans == null || !treatmentPlans.Any())
                {
                    return NotFound(new { Code = "not_found", Message = "Không tìm thấy kế hoạch điều trị cho khách hàng." });
                }

                return Ok(treatmentPlans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Code = "server_error", Message = "Có lỗi xảy ra trong quá trình xử lý yêu cầu.", Error = ex.Message });
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
        [HttpPost("Create")]
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
                return StatusCode(500, new { Code = "server_error", Message = "Có lỗi xảy ra trong quá trình xử lý yêu cầu.", Error = ex.Message });
            }

            return BadRequest("Không thể tạo kế hoạch điều trị.");
        }

        // GET: api/TreatmentPlan/Detail/{treatmentPlanId}
        [HttpGet("Detail/{treatmentPlanId}")]
        public async Task<IActionResult> GetTreatmentPlanDetail(int treatmentPlanId)
        {
            try
            {
                var treatmentPlan = await _treatmentService.GetTreatmentPlanDetailsAsync(treatmentPlanId);

                if (treatmentPlan == null)
                {
                    return NotFound(new { Code = "not_found", Message = "Kế hoạch điều trị không tồn tại." });
                }

                return Ok(treatmentPlan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Code = "server_error", Message = "Có lỗi xảy ra trong quá trình xử lý yêu cầu.", Error = ex.Message });
            }
        }

        // PUT: api/TreatmentPlan/{treatmentId}
        [HttpPut("Update/{treatmentId}")]
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
                    return Ok(new { Message = "Cập nhật kế hoạch điều trị thành công." });
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = $"Không tìm thấy kế hoạch điều trị với ID:{treatmentId} .", Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Có lỗi xảy ra trong quá trình xử lý yêu cầu.", Error = ex.Message });
            }

            return BadRequest("Không thể cập nhật kế hoạch điều trị.");
        }

        // DELETE: api/TreatmentPlan/{treatmentId}
        [HttpDelete("Delete/{treatmentId}")]
        public async Task<IActionResult> DeleteTreatmentPlan(int treatmentId)
        {
            try
            {
                var result = await _treatmentService.DeleteTreatmentPlanAsync(treatmentId);
                if (result)
                {
                    return Ok(new { Message = "Đã xóa kế hoạch điều trị thành công." });
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = $"Không tìm thấy kế hoạch điều trị với :{treatmentId}.", Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Có lỗi xảy ra trong quá trình xử lý yêu cầu.", Error = ex.Message });
            }

            return BadRequest("Không thể xóa kế hoạch điều trị.");
        }
    }
}
