using DentalBooking.ModelViews.ServiceModelViews;

namespace DentalBooking.Contract.Services
{
    public interface IServiceServices
    {
        Task<ServiceResponeModelViews> CreateServiceAsync(ServiceRequestModelView model);
        Task<IEnumerable<ServiceResponeModelViews>> GetAllServicesAsync();
        Task<ServiceResponeModelViews> GetServiceByIdAsync(int id);
        Task<bool> UpdateServiceAsync(int id, ServiceRequestModelView model);
        Task<bool> DeleteServiceAsync(int id);
    }
}
