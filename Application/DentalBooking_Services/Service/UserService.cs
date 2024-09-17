using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository.Interface;
using DentalBooking.ModelViews.UserModelViews;
using DentalBooking_Contract_Services.Interface;


namespace DentalBooking_Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Lấy tất cả người dùng từ cơ sở dữ liệu
        public async Task<IList<UserResponseModel>> GetAll()
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var users = await userRepository.GetAllAsync();

            var userResponseModels = users.Select(user => new UserResponseModel
            {
                Id = user.Id.ToString(),
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
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

    }
}
