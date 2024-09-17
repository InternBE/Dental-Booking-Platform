using DentalBooking.Contract.Repository.Entity;
using DentalBooking_Contract_Services.Interface;
using DentalBooking.ModelViews.UserModelViews;
using System;
using System.Threading.Tasks;
using DentalBooking.Contract.Repository;

namespace DentalBooking_Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Cập nhật User (nhận UserUpdateModel từ controller)
        public async Task UpdateUserAsync(UserUpdateModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel), "UserUpdateModel object cannot be null");
            }

            // Tìm kiếm người dùng trong cơ sở dữ liệu
            var existingUser = await _unitOfWork.Users.GetByIdAsync(userModel.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Cập nhật các thông tin từ UserUpdateModel sang thực thể User
            existingUser.FullName = userModel.FullName;
            existingUser.Email = userModel.Email;
            existingUser.PhoneNumber = userModel.PhoneNumber;
            existingUser.ClinicId = userModel.ClinicId;

            // Thực hiện cập nhật và lưu vào cơ sở dữ liệu
            _unitOfWork.Users.Update(existingUser);
            await _unitOfWork.CompleteAsync();
        }

        // Xóa User
        public async Task DeleteUserAsync(int id)
        {
            // Tìm người dùng theo ID
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Thực hiện xóa người dùng
            _unitOfWork.Users.Delete(user);
            await _unitOfWork.CompleteAsync();
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
