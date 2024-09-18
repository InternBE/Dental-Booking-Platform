using DentalBooking.Contract.Repository;
using DentalBooking.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

        public IUserRepository Users { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
