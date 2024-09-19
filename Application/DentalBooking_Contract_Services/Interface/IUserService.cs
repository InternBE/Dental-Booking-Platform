using DentalBooking.Contract.Repository.Entity;
using DentalBooking.ModelViews.UserModelViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalBooking_Contract_Services.Interface
{
    public interface IUserService
    {
        // Trả về danh sách người dùng dưới dạng UserResponseModel
        Task<IList<UserResponseModel>> GetAll();

        // Tạo người dùng mới từ UserRequestModel và trả về UserResponseModel
        Task<UserResponseModel> Create(UserRequestModel userRequest);

        // Các phương thức bất đồng bộ
        Task<IEnumerable<User>> GetAllUsersAsync(); // Có thể được giữ nếu có nhu cầu khác biệt
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
