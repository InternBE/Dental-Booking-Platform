using DentalBooking.Contract.Repository.Entity;
using DentalBooking.ModelViews.UserModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking_Contract_Services.Interface
{
    public interface IUserService
    {
        Task<IList<UserResponseModel>> GetAll(); // Trả về UserResponseModel thay vì User
        Task<UserResponseModel> Create(UserRequestModel userRequest); // Nhận vào UserRequestModel
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
