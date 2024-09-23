using DentalBooking.Contract.Repository;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Services;
using DentalBooking.Core.Utils;
using DentalBooking.ModelViews.AppointmentModelViews;
using DentalBooking_Contract_Services.Interface;
using Hangfire;

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
        //Đăng ký lịch khám 1 lần
        public async Task<AppointmentResponeModelViews> BookOneTimeAppointmentAsync(AppointmentRequestModelView model)
        {
            var appointmentEntity = new Appointment
            {
                UserId = model.UserId,
                ClinicId = model.ClinicId,
                TreatmentPlanId = model.TreatmentPlanId,
                AppointmentDate = model.AppointmentDate,
                Status = model.Status
            };

            var repository = _unitOfWork.GetRepository<Appointment>();
            await repository.InsertAsync(appointmentEntity);
            await _unitOfWork.SaveAsync();

            
            ScheduleReminder(appointmentEntity);

            return new AppointmentResponeModelViews
            {
                AppointmentDate = appointmentEntity.AppointmentDate,
                Status = appointmentEntity.Status,
                UserId = appointmentEntity.UserId,
                ClinicId = appointmentEntity.ClinicId,
                TreatmentPlanId = appointmentEntity.TreatmentPlanId
            };
        }

        // Đặt lịch nhắc nhở 1 ngày trước ngày hạn
        private void ScheduleReminder(Appointment appointmentEntity)
        {
            var reminderDate = appointmentEntity.AppointmentDate.AddDays(-1);
            //Sử dụng Hangfire.Core để tạo một bộ lập lịch công việc nền
            BackgroundJob.Schedule(() => SendReminder(appointmentEntity.UserId, appointmentEntity.AppointmentDate), reminderDate);
        }

        private void SendReminder(int userId, DateTime appointmentDate)
        {
           
        }
        //Điều trị định kỳ
        public async Task<List<AppointmentResponeModelViews>> BookPeriodicAppointmentsAsync(AppointmentRequestModelView model, int months)
        {
            var repository = _unitOfWork.GetRepository<Appointment>();
            var responseAppointments = new List<AppointmentResponeModelViews>();

            for (int i = 0; i < months; i++)
            {
                var appointmentEntity = new Appointment
                {
                    UserId = model.UserId,
                    ClinicId = model.ClinicId,
                    TreatmentPlanId = model.TreatmentPlanId,
                    AppointmentDate = model.AppointmentDate.AddMonths(i),
                    Status = model.Status
                };

                await repository.InsertAsync(appointmentEntity);
                responseAppointments.Add(new AppointmentResponeModelViews
                {
                    AppointmentDate = appointmentEntity.AppointmentDate,
                    Status = appointmentEntity.Status,
                    UserId = appointmentEntity.UserId,
                    ClinicId = appointmentEntity.ClinicId,
                    TreatmentPlanId = appointmentEntity.TreatmentPlanId
                });
            }

            await _unitOfWork.SaveAsync();
            return responseAppointments;
        }
        
        public async Task<IEnumerable<AppointmentResponeModelViews>> AllAppointmentsByUserIdAsync(int UserId)
        {
            var repository = _unitOfWork.GetRepository<Appointment>();

            // Chờ để nhận danh sách các cuộc hẹn
            IEnumerable<Appointment> lAppointment = await repository.FindAsync(x => x.UserId == UserId);

            return lAppointment.Select(appointment => new AppointmentResponeModelViews {
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                UserId = appointment.UserId,
                ClinicId = appointment.ClinicId,
                TreatmentPlanId = appointment.TreatmentPlanId
            }
            );
           

            
        }
    }
}
