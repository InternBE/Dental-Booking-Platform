using DentalBooking.Contract.Repository;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository.IUOW;
using DentalBooking.Repository.Context;
using System;
using System.Threading.Tasks;

namespace DentalBooking.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Clinic> _clinicRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IGenericRepository<Clinic> ClinicRepository =>
            _clinicRepository ??= new GenericRepository<Clinic>(_context);

        public IUserRepository Users =>
            _userRepository ??= new UserRepository(_context); // Giả sử bạn có lớp UserRepository

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
