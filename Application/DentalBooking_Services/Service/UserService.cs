using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository;
using DentalBooking.ModelViews.UserModelViews;
using DentalBooking_Contract_Services.Interface;
using Microsoft.AspNetCore.Identity;
using DentalBooking.Repository.Context;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DatabaseContext _dbContext;

    public UserService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, DatabaseContext dbContext)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
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

    // Phương thức phân trang mới: Lấy danh sách người dùng có phân trang
    public async Task<IEnumerable<UserResponseModel>> GetPaginatedUsersAsync(int pageNumber, int pageSize)
    {
        var users = await _dbContext.Users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return users.Select(user => new UserResponseModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            ClinicId = user.ClinicId,
        });
    }

    // Phương thức mới: Lấy tổng số lượng người dùng
    public async Task<int> GetTotalUsersCountAsync()
    {
        return await _dbContext.Users.CountAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        // Kiểm tra nếu người dùng không tồn tại
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        return user;
    }

    public Task AddUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateUserAsync(User user)
    {
        var existingUser = await _dbContext.Users.FindAsync(user.Id);

        if (existingUser == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        existingUser.FullName = user.FullName;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.ClinicId = user.ClinicId;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ApproveDoctorAsync(string doctorId)
    {
        var doctor = await _userManager.FindByIdAsync(doctorId);
        if (doctor == null) return false;

        doctor.IsApproved = true;
        await _userManager.UpdateAsync(doctor);
        return true;
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }
}
