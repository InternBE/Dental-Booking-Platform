using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Core.Base;
using DentalBooking.ModelViews.ModelViews.UserModelViews;
using DentalBooking.ModelViews.UserModelViews;
using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService userService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // Lấy danh sách tất cả người dùng (Chỉ Admin mới có quyền truy cập) với phân trang
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new { message = "Page number and page size must be greater than 0." });
            }

            var users = await _userService.GetPaginatedUsersAsync(pageNumber, pageSize);
            var totalRecords = await _userService.GetTotalUsersCountAsync();

            var response = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = users
            };

            return Ok(response);
        }

        // Lấy thông tin người dùng theo ID
        [Authorize(Roles = "ADMIN,CUSTOMER,DENTIST")]
        //[Authorize(Roles = "Admin,Customer,Dentist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng" });
            }
            return Ok(BaseResponse<UserResponseModel>.OkResponse(new UserResponseModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ClinicId = user.ClinicId
            }));
        }

        // Đăng ký người dùng mới
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            UserRequestModel requestModel = new UserRequestModel() { Email = model.Email, ClinicId = model.ClinicId };

            var userResponse = await _userService.Create(requestModel);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, UserId = userResponse.Id };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "ADMIN");
                return Ok(new { message = "Đăng ký tài khoản thành công" });
            }

            return BadRequest(result.Errors);
        }

        // Đăng nhập và trả về JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user, roles.FirstOrDefault());

                return Ok(new
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddHours(1), // Token hết hạn sau 1 giờ
                    Role = roles.FirstOrDefault(),
                    UserId = user.Id // Trả về ID người dùng
                });
            }

            return Unauthorized("Email hoặc mật khẩu không đúng");
        }

        // Cập nhật thông tin người dùng
        [Authorize(Roles = "ADMIN,CUSTOMER,DENTIST")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userModel.Id)
            {
                return BadRequest("Id Người dùng không hợp lệ");
            }

            try
            {
                var userEntity = new User
                {
                    Id = userModel.Id,
                    FullName = userModel.FullName,
                    Email = userModel.Email,
                    PhoneNumber = userModel.PhoneNumber,
                    ClinicId = userModel.ClinicId
                };

                await _userService.UpdateUserAsync(userEntity);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy người " });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Xóa người dùng
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy người dùng" });
            }
        }

        // Tạo JWT token
        private string GenerateJwtToken(ApplicationUser user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
