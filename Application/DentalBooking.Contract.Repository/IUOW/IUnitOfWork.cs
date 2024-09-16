using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository.IUOW;

namespace DentalBooking.Contract.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Clinic> ClinicRepository { get; }
        Task SaveAsync();
    }
}
