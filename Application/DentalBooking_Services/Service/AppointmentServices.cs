using DentalBooking.Contract.Repository;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Services;
using DentalBooking.Core.Utils;
using DentalBooking.ModelViews.AppointmentModelViews;
using DentalBooking_Contract_Services.Interface;

namespace DentalBooking_Services.Service
{
    public class AppointmentServices:IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AppointmentResponeModelViews> CreateAppointmentAsync(AppointmentRequestModelView model)
        {
            var appointmentEntity = new Appointment
            {
                AppointmentDate = model.AppointmentDate,
                Status = model.Status,
                UserId = model.UserId,
                ClinicId = model.ClinicId,
                TreatmentPlanId = model.TreatmentPlanId
            };

            var repository = _unitOfWork.GetRepository<Appointment>();
            await repository.InsertAsync(appointmentEntity);
            await _unitOfWork.SaveAsync();

            return new AppointmentResponeModelViews
            {
                AppointmentDate = appointmentEntity.AppointmentDate,
                Status = appointmentEntity.Status,
                UserId = appointmentEntity.UserId,
                ClinicId = appointmentEntity.ClinicId,
                TreatmentPlanId = appointmentEntity.TreatmentPlanId,
               
            };
        }

        public async Task<IEnumerable<AppointmentResponeModelViews>> GetAllAppointmentsAsync()
        {
            var repository = _unitOfWork.GetRepository<Appointment>();

            // Chờ để nhận danh sách các cuộc hẹn
            var appointments = await repository.GetAllAsync();

            // Sử dụng phương thức Where trên danh sách đã nhận
            var filteredAppointments = appointments.Where(a => a.DeletedTime == null).ToList();

            return filteredAppointments.Select(appointment => new AppointmentResponeModelViews
            {
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                UserId = appointment.UserId,
                ClinicId = appointment.ClinicId,
                TreatmentPlanId = appointment.TreatmentPlanId
            }).ToList();
        }

        public async Task<AppointmentResponeModelViews> GetAppointmentByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Appointment>();
            var appointment = await repository.GetByIdAsync(id);

            if (appointment == null || appointment.DeletedTime != null) return null;

            return new AppointmentResponeModelViews
            {
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                UserId = appointment.UserId,
                ClinicId = appointment.ClinicId,
                TreatmentPlanId = appointment.TreatmentPlanId
            };
        }

        public async Task<bool> UpdateAppointmentAsync(int id, AppointmentRequestModelView model)
        {
            var repository = _unitOfWork.GetRepository<Appointment>();
            var appointment = await repository.GetByIdAsync(id);

            if (appointment == null || appointment.DeletedTime != null) return false;

            appointment.AppointmentDate = model.AppointmentDate;
            appointment.Status = model.Status;
            appointment.UserId = model.UserId;
            appointment.ClinicId = model.ClinicId;
            appointment.TreatmentPlanId = model.TreatmentPlanId;

            repository.Update(appointment);
            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Appointment>();
            var appointment = await repository.GetByIdAsync(id);

            if (appointment == null) return false; // Không tìm thấy cuộc hẹn

            // Nếu bản ghi chưa bị xóa mềm, thực hiện xóa mềm
            if (appointment.DeletedTime == null)
            {
                appointment.DeletedTime = DateTimeOffset.Now; // Gắn thời gian xóa mềm
                appointment.DeletedBy = "Tên người xóa"; // Thay thế bằng thông tin người dùng hiện tại nếu có
                repository.Update(appointment); // Cập nhật bản ghi để xóa mềm
            }
            else
            {
                // Nếu bản ghi đã bị xóa mềm, thực hiện xóa vĩnh viễn
                repository.Delete(id); // Xóa bản ghi khỏi DB
            }

            await _unitOfWork.SaveAsync(); // Lưu thay đổi
            return true;
        }


    }
}
