using DentalBooking.Contract.Repository.Entity;
using DentalBooking_Contract_Services.Interface;
using DentalBooking.ModelViews.UserModelViews;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalBooking.Contract.Repository;

namespace DentalBooking_Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Lấy danh sách tất cả người dùng
        public async Task<IList<UserResponseModel>> GetAll()
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var users = await userRepository.GetAllAsync();

            var userResponseModels = users.Select(user => new UserResponseModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ClinicId = user.ClinicId,
            }).ToList();

            return userResponseModels;
        }

        // Tạo mới người dùng
        public async Task<UserResponseModel> Create(UserRequestModel userRequest)
        {
            var user = new User
            {
                FullName = userRequest.FullName,
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNumber,
                ClinicId = userRequest.ClinicId,
            };

            await _unitOfWork.GetRepository<User>().InsertAsync(user);
            await _unitOfWork.SaveAsync();

            return new UserResponseModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        // Cập nhật thông tin người dùng
        public async Task UpdateUserAsync(UserUpdateModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel), "UserUpdateModel object cannot be null");
            }

            var existingUser = await _unitOfWork.Users.GetByIdAsync(userModel.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            existingUser.FullName = userModel.FullName;
            existingUser.Email = userModel.Email;
            existingUser.PhoneNumber = userModel.PhoneNumber;
            existingUser.ClinicId = userModel.ClinicId;

            _unitOfWork.Users.Update(existingUser);
            await _unitOfWork.CompleteAsync();
        }

        // Xóa người dùng
        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.CompleteAsync();
        }

        // Các phương thức chưa được triển khai
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
