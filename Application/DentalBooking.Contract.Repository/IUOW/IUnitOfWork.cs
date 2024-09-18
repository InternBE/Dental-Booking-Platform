using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository.IUOW;
using System;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        void Save();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
        IGenericRepository<Clinic> ClinicRepository { get; }
        IUserRepository Users { get; }
        Task SaveAsync();
        Task<int> CompleteAsync();
    }
}
