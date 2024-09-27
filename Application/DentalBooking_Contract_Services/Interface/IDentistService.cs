using DentalBooking.Contract.Repository.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DentalBooking_Contract_Services.Interface
{
    public interface IDentistService
    {
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task<Appointment> ScheduleAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(int id);
        Task<Appointment> ScheduleFollowUpAppointmentAsync(int appointmentId); // Thêm phương thức này
    }
}
