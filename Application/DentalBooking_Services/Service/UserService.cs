using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository;
using DentalBooking.ModelViews.UserModelViews;
using DentalBooking_Contract_Services.Interface;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Phương thức GetAll để trả về danh sách UserResponseModel
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

    // Phương thức Create để tạo người dùng mới
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
}
