using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository.IUOW;

namespace DentalBooking.Contract.Repository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> FindAsync(Func<object, bool> value);
        Task<IEnumerable<Appointment>> GetAppointmentsByDentistIdAsync(int dentistId);
        Task<IEnumerable<Appointment>> GetPaginatedAppointmentsAsync(int pageNumber, int pageSize);
        Task<int> GetTotalAppointmentsCountAsync();
    }
}