using DentalBooking.Contract.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task AddAsync(User user);
        void Delete(User user);

        // Các phương thức đặc thù cho User nếu cần thêm.
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        void Update(User user);
    }

    public interface IGenericRepository<T>
    {
    }
}
