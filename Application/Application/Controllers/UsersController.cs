using DentalBooking.Core.Base;
using DentalBooking.ModelViews.UserModelViews;
using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet()]
        public async Task<IActionResult> Login(int index = 1, int pageSize = 10)
        {
            IList<UserResponseModel> a = await _userService.GetAll();
            return Ok(BaseResponse<IList<UserResponseModel>>.OkResponse(a));
        }

        
    }
}
