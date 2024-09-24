using DentalBooking.ModelViews.TreatmentModels;
using DentalBooking.ModelViews.TreatmentPlanModels;
using System.Threading.Tasks;

namespace DentalBooking_Contract_Services.Interface
{
    public interface ITreatmentService
    {
        // Lấy tất cả cuộc hẹn
        Task<IEnumerable<TreatmentPlanResponseModelView>> GetAllAppointmentsAsync();
        // Lấy chi tiết cuộc hẹn theo ID
        Task<TreatmentPlanResponseModelView> GetAppointmentDetailsAsync(int appointmentId);

        // Lấy kế hoạch điều trị theo mã khách hàng
        Task<TreatmentPlanResponseModelView> GetTreatmentPlanAsync(int customerId);

        // Cập nhật kế hoạch điều trị
        Task<bool> UpdateTreatmentPlanAsync(int treatmentId, TreatmentPlanRequestModelView treatmentRequestModel);

        // Thêm mới kế hoạch điều trị
        Task<bool> CreateTreatmentPlanAsync(TreatmentPlanRequestModelView treatmentRequestModel);

        // Xóa kế hoạch điều trị theo ID
        Task<bool> DeleteTreatmentPlanAsync(int treatmentId);

        Task SendTreatmentPlanToCustomerAsync(int customerId, int doctorId);
    }
}
