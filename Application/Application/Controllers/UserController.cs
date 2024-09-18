using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Core.Base;
using DentalBooking.ModelViews.UserModelViews;
using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRequestModel userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userService.Create(userRequest);
            return CreatedAtAction(nameof(Create), new { id = createdUser.Id }, BaseResponse<UserResponseModel>.OkResponse(createdUser));
        }
        // Cập nhật thông tin User (sử dụng UserUpdateModel)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userModel.Id)
            {
                return BadRequest("User ID mismatch");
            }

            try
            {
                // Ánh xạ từ UserUpdateModel sang thực thể User
                var userEntity = new User
                {
                    Id = userModel.Id,
                    FullName = userModel.FullName,
                    Email = userModel.Email,
                    PhoneNumber = userModel.PhoneNumber,
                    ClinicId = userModel.ClinicId
                };

                await _userService.UpdateUserAsync(userEntity);  // Cập nhật người dùng
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found" });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Xóa User (Không cần ModelView vì chỉ sử dụng ID)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);  // Xóa người dùng
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found" });
            }
        }
    }
}
