using DentalBooking.ModelViews.AppointmentModelViews;


namespace DentalBooking_Contract_Services.Interface
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentResponeModelViews>> GetAllAppointmentsAsync();
        Task<IEnumerable<AppointmentResponeModelViews>> AllAppointmentsByUserIdAsync(int UserId);
        Task<AppointmentResponeModelViews> GetAppointmentByIdAsync(int id);
        Task<AppointmentResponeModelViews> CreateAppointmentAsync(AppointmentRequestModelView model);
        Task<bool> UpdateAppointmentAsync(int id, AppointmentRequestModelView model);
        Task<bool> DeleteAppointmentAsync(int id);
        Task<AppointmentResponeModelViews> BookOneTimeAppointmentAsync(AppointmentRequestModelView model);
        Task<List<AppointmentResponeModelViews>> BookPeriodicAppointmentsAsync(AppointmentRequestModelView model, int months);
    }
}
