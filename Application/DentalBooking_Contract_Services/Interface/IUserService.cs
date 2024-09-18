using DentalBooking.Contract.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DentalBooking_Contract_Services.Interface
{
    public interface IUserService
    {
        Task<IList<User>> GetAll();
        Task<User> Create(User userRequest);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }

}
