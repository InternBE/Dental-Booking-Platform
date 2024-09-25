using DentalBooking.Contract.Services;
using DentalBooking.ModelViews.ServiceModelViews;
using Microsoft.AspNetCore.Mvc;

namespace DentalBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceServices _serviceServices;

        public ServiceController(IServiceServices serviceServices)
        {
            _serviceServices = serviceServices;
        }

        // GET: api/Service
        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceServices.GetAllServicesAsync();
            return Ok(services);
        }

        // GET: api/Service/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _serviceServices.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound(new { message = $"Dịch vụ với ID {id} không tồn tại." });
            }
            return Ok(service);
        }

        // POST: api/Service
        [HttpPost("Create")]
        public async Task<IActionResult> CreateService([FromBody] ServiceRequestModelView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdService = await _serviceServices.CreateServiceAsync(model);
            return CreatedAtAction(nameof(GetServiceById), new { id = createdService.ServiceName }, createdService);
        }

        // PUT: api/Service/{id}
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] ServiceRequestModelView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceServices.UpdateServiceAsync(id, model);
            if (!result)
            {
                return BadRequest(new { message = "Sửa dịch vụ không thành công do lỗi hệ thống." });
            }

            return Ok(new { message = $"Dịch vụ với ID {id} đã được sửa thành công." });
        }

        // DELETE: api/Service/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await _serviceServices.DeleteServiceAsync(id);
            if (!result)
            {
                return BadRequest(new { message = "Xóa dịch vụ không thành công do lỗi hệ thống." });
            }

            return Ok(new { message = $"Dịch vụ với ID {id} đã được xóa thành công." });
        }
    }
}
