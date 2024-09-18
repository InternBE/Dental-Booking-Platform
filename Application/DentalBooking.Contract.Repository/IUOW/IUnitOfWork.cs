using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository.IUOW;
using System;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Clinic> ClinicRepository { get; }
        IUserRepository Users { get; }
        Task SaveAsync();
        Task<int> CompleteAsync();
    }
}
