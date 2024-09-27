using DentalBooking.ModelViews.TreatmentModels;
using DentalBooking.ModelViews.TreatmentPlanModels;
using System.Threading.Tasks;

namespace DentalBooking_Contract_Services.Interface
{
    public interface ITreatmentPlanService
    {
        // Lấy tất cả kế hoạch điều trị (không phân trang)
        Task<IEnumerable<TreatmentPlanResponseModelView>> GetAllTreatmentPlansAsync();

        // Lấy danh sách kế hoạch điều trị với phân trang
        Task<IEnumerable<TreatmentPlanResponseModelView>> GetPaginatedTreatmentPlansAsync(int pageNumber, int pageSize);

        // Lấy tổng số lượng kế hoạch điều trị
        Task<int> GetTotalTreatmentPlansCountAsync();

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

        // Gửi kế hoạch điều trị tới khách hàng
        Task SendTreatmentPlanToCustomerAsync(int customerId, int doctorId);
    }
}
