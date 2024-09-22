using DentalBooking.Contract.Services;
using DentalBooking_Services.Service;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceServices _services;
        public HomeController(IServiceServices serviceServices)
        {
            _services = serviceServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _services.GetAllServicesAsync();
            return Ok(services);
        }
    }
}
