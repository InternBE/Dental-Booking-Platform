using DentalBooking.Contract.Repository;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository.IUOW;
using DentalBooking.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DentalBooking.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Clinic> _clinicRepository;
        private IUserRepository _userRepository;
        private bool _disposed = false;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        // Thay đổi kiểu trả về của ClinicRepository thành IGenericRepository<Clinic>
        public IGenericRepository<Clinic> ClinicRepository =>
            _clinicRepository ??= new GenericRepository<Clinic>(_context);

        public IUserRepository Users =>
            _userRepository ??= new UserRepository(_context);

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

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
