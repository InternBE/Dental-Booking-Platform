using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository.IUOW;
using System;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        // Generic repository methods
        IGenericRepository<T> GetRepository<T>() where T : class;
        IGenericRepository<Clinic> ClinicRepository { get; }
        IUserRepository Users { get; }

        // Transaction methods
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();

        // Save methods
        void Save();
        Task SaveAsync();
        Task<int> CompleteAsync();
    }
}
