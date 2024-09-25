using DentalBooking.ModelViews.TreatmentModels;
using DentalBooking.ModelViews.TreatmentPlanModels;
using System.Threading.Tasks;

namespace DentalBooking_Contract_Services.Interface
{
    public interface ITreatmentPlanService
    {
        Task<IEnumerable<TreatmentPlanResponseModelView>> GetAllTreatmentPlansAsync();

        // Lấy chi tiết kế hoạch điều trị theo ID
        Task<TreatmentPlanResponseModelView> GetTreatmentPlanDetailsAsync(int treatmentPlanId);

        // Lấy tất cả kế hoạch điều trị của khách hàng
        Task<IEnumerable<TreatmentPlanResponseModelView>> GetAllTreatmentPlansForCustomerAsync(int customerId);

        // Cập nhật kế hoạch điều trị
        Task<bool> UpdateTreatmentPlanAsync(int treatmentId, TreatmentPlanRequestModelView treatmentRequestModel);

        // Thêm mới kế hoạch điều trị
        Task<bool> CreateTreatmentPlanAsync(TreatmentPlanRequestModelView treatmentRequestModel);

        // Xóa kế hoạch điều trị theo ID
        Task<bool> DeleteTreatmentPlanAsync(int treatmentId);

        Task SendTreatmentPlanToCustomerAsync(int customerId, int doctorId);
    }
}
