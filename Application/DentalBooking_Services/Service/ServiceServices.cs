using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository.Interface;
using DentalBooking.Contract.Services;
using DentalBooking.ModelViews.ServiceModelViews;

namespace DentalBooking_Services.Service
{
    public class ServiceServices : IServiceServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponeModelViews> CreateServiceAsync(ServiceRequestModelView model)
        {
            var serviceEntity = new DentalBooking.Contract.Repository.Entity.Services
            {
                ServiceName = model.ServiceName,
                Description = model.Description,
                Price = model.Price
            };

            var repository = _unitOfWork.GetRepository<DentalBooking.Contract.Repository.Entity.Services>();
            await repository.InsertAsync(serviceEntity);
            await _unitOfWork.SaveAsync();

            return new ServiceResponeModelViews
            {
                ServiceName = serviceEntity.ServiceName,
                Description = serviceEntity.Description,
                Price = serviceEntity.Price
            };
        }

        public async Task<IEnumerable<ServiceResponeModelViews>> GetAllServicesAsync()
        {
            var repository = _unitOfWork.GetRepository<DentalBooking.Contract.Repository.Entity.Services>();
            var services = await repository.GetAllAsync();

            return services.Select(service => new ServiceResponeModelViews
            {
                ServiceName = service.ServiceName,
                Description = service.Description,
                Price = service.Price
            }).ToList();
        }

        public async Task<ServiceResponeModelViews> GetServiceByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<DentalBooking.Contract.Repository.Entity.Services>();
            var service = await repository.GetByIdAsync(id);

            if (service == null) return null;

            return new ServiceResponeModelViews
            {
                ServiceName = service.ServiceName,
                Description = service.Description,
                Price = service.Price
            };
        }

        public async Task<bool> UpdateServiceAsync(int id, ServiceRequestModelView model)
        {
            var repository = _unitOfWork.GetRepository<DentalBooking.Contract.Repository.Entity.Services>();
            var service = await repository.GetByIdAsync(id);

            if (service == null) return false;

            service.ServiceName = model.ServiceName;
            service.Description = model.Description;
            service.Price = model.Price;

            repository.Update(service);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<DentalBooking.Contract.Repository.Entity.Services>();
            var service = await repository.GetByIdAsync(id);

            if (service == null) return false;

            repository.Delete(service);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
