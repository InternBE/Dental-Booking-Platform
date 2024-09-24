﻿using DentalBooking.ModelViews.AppointmentModelViews;


namespace DentalBooking_Contract_Services.Interface
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentResponeModelViews>> GetAllAppointmentsAsync();
        Task<AppointmentResponeModelViews> GetAppointmentByIdAsync(int id);
        Task<AppointmentResponeModelViews> CreateAppointmentAsync(AppointmentRequestModelView model);
        Task<bool> UpdateAppointmentAsync(int id, AppointmentRequestModelView model);
        Task<bool> DeleteAppointmentAsync(int id);

        Task<DateTime?> SuggestNextAppointment(int userId, int treatmentPlanId);
    }
}
