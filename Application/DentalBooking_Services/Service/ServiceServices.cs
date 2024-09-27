using DentalBooking.Contract.Repository;
using DentalBooking.Contract.Repository.Entity;
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

            // Lọc các dịch vụ chưa bị xóa mềm
            var filteredServices = services.Where(s => s.DeletedTime == null).ToList();

            return filteredServices.Select(service => new ServiceResponeModelViews
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

            if (service == null || service.DeletedTime != null) return null;

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

            if (service == null || service.DeletedTime != null) return false; // Kiểm tra xóa mềm

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

            if (service == null) return false; // Không tìm thấy bản ghi

            // Nếu bản ghi chưa bị xóa mềm, thực hiện xóa mềm
            if (service.DeletedTime == null)
            {
                service.DeletedTime = DateTimeOffset.Now; // Gắn thời gian xóa mềm
                service.DeletedBy = "Tên người xóa"; // Thay thế bằng thông tin người dùng hiện tại nếu có
                repository.Update(service); // Cập nhật trạng thái xóa mềm
            }
            else
            {
                // Nếu bản ghi đã bị xóa mềm, thực hiện xóa vĩnh viễn
                repository.Delete(id); // Xóa bản ghi khỏi DB
            }

            await _unitOfWork.SaveAsync(); // Lưu thay đổi
            return true;
        }

        // Phương thức mới để hỗ trợ phân trang
        public async Task<IEnumerable<ServiceResponeModelViews>> GetPaginatedServicesAsync(int pageNumber, int pageSize)
        {
            var repository = _unitOfWork.GetRepository<DentalBooking.Contract.Repository.Entity.Services>();
            var services = await repository.GetAllAsync();

            // Lọc các dịch vụ chưa bị xóa mềm và phân trang
            var filteredServices = services.Where(s => s.DeletedTime == null)
                                           .Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToList();

            return filteredServices.Select(service => new ServiceResponeModelViews
            {
                ServiceName = service.ServiceName,
                Description = service.Description,
                Price = service.Price
            }).ToList();
        }

        // Phương thức mới để lấy tổng số lượng dịch vụ
        public async Task<int> GetTotalServicesCountAsync()
        {
            var repository = _unitOfWork.GetRepository<DentalBooking.Contract.Repository.Entity.Services>();
            var services = await repository.GetAllAsync();

            // Đếm các dịch vụ chưa bị xóa mềm
            return services.Count(s => s.DeletedTime == null);
        }
    }
}
