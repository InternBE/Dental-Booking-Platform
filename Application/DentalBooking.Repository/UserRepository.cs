using DentalBooking.Contract.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalBooking.Repository.Context;
using DentalBooking.Contract.Repository.IUOW;

namespace DentalBooking.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        public Task AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IUserRepository.AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        void IUserRepository.Delete(User user)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<User>> IUserRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<User> IUserRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        void IUserRepository.Update(User user)
        {
            throw new NotImplementedException();
        }
        // Triển khai các phương thức nếu cần thêm.
    }

}
