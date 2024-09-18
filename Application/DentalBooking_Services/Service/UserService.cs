using DentalBooking.Contract.Repository.Entity;
using DentalBooking_Contract_Services.Interface;
using DentalBooking.ModelViews.UserModelViews;
using System;
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
        public async Task<UserResponseModel> Create(UserRequestModel userRequest)
        {
            // Tạo đối tượng User từ dữ liệu yêu cầu
            var user = new User
            {
                FullName = userRequest.FullName,
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNumber,
                ClinicId = userRequest.ClinicId,
            };

            // Thêm người dùng vào repository
            await _unitOfWork.GetRepository<User>().InsertAsync(user);
            await _unitOfWork.SaveAsync();

            // Chuyển đổi dữ liệu từ entity sang DTO (UserResponseModel)
            return new UserResponseModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
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
