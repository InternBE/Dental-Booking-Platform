using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository;
using DentalBooking.ModelViews.UserModelViews;
using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Identity;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
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

    // Các phương thức khác
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

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ApproveDoctorAsync(string doctorId)
    {
        var doctor = await _userManager.FindByIdAsync(doctorId);
        if (doctor == null) return false;

        doctor.IsApproved = true;
        await _userManager.UpdateAsync(doctor);
        return true;
    }
}
