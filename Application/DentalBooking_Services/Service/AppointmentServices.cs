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

        public async Task<DateTime?> SuggestNextAppointment(int userId, int treatmentPlanId)
        {
            // Lấy kế hoạch điều trị
            var treatmentPlanRepository = _unitOfWork.GetRepository<TreatmentPlans>();
            var treatmentPlan = await treatmentPlanRepository.GetByIdAsync(treatmentPlanId);

            if (treatmentPlan == null || treatmentPlan.EndDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now)
            {
                // Kế hoạch điều trị không tồn tại hoặc đã kết thúc
                return null;
            }

            // Lấy tất cả các cuộc hẹn và sau đó lọc các cuộc hẹn liên quan đến bệnh nhân và kế hoạch điều trị
            var appointmentRepository = _unitOfWork.GetRepository<Appointment>();
            var allAppointments = await appointmentRepository.GetAllAsync(); // Lấy tất cả cuộc hẹn

            // Lọc cuộc hẹn của bệnh nhân theo kế hoạch điều trị và sắp xếp theo ngày gần nhất
            var lastAppointment = allAppointments
                .Where(a => a.UserId == userId && a.TreatmentPlanId == treatmentPlanId && a.DeletedTime == null)
                .OrderByDescending(a => a.AppointmentDate)
                .FirstOrDefault();

            // Tính toán ngày tái khám tiếp theo
            DateTime nextAppointmentDate;

            if (lastAppointment == null)
            {
                // Nếu không có cuộc hẹn trước đó, lấy StartDate của kế hoạch điều trị
                nextAppointmentDate = treatmentPlan.StartDate.ToDateTime(TimeOnly.MinValue);
            }
            else
            {
                // Nếu đã có cuộc hẹn, sử dụng ngày gần nhất và thêm khoảng tái khám định kỳ
                TimeSpan recurrencePeriod = TimeSpan.FromDays(30);
                nextAppointmentDate = lastAppointment.AppointmentDate.Add(recurrencePeriod);
            }

            // Kiểm tra nếu ngày tái khám tiếp theo vượt quá EndDate của kế hoạch điều trị
            if (nextAppointmentDate > treatmentPlan.EndDate.ToDateTime(TimeOnly.MinValue))
            {
                return null; // Không đề xuất tái khám nếu vượt quá ngày kết thúc điều trị
            }

            // Kiểm tra trùng thời gian với các cuộc hẹn khác của bệnh nhân
            bool isConflict;
            int maxRetries = 10; // Giới hạn số lần thử để tìm một khoảng thời gian không trùng

            do
            {
                isConflict = await CheckAppointmentConflict(userId, nextAppointmentDate);

                if (isConflict)
                {
                    // Nếu có trùng lặp, thêm một khoảng thời gian (ví dụ: 30 phút)
                    nextAppointmentDate = nextAppointmentDate.AddMinutes(30);
                }

                maxRetries--;

                // Kiểm tra nếu thời gian mới vượt quá EndDate
                if (nextAppointmentDate > treatmentPlan.EndDate.ToDateTime(TimeOnly.MinValue))
                {
                    return null; // Không có thời gian phù hợp trong khoảng thời gian của kế hoạch điều trị
                }

            } while (isConflict && maxRetries > 0);

            return nextAppointmentDate;
        }

        private async Task<bool> CheckAppointmentConflict(int userId, DateTime proposedDate)
        {
            var appointmentRepository = _unitOfWork.GetRepository<Appointment>();

            // Lấy tất cả các cuộc hẹn
            var allAppointments = await appointmentRepository.GetAllAsync();

            // Kiểm tra nếu có cuộc hẹn nào trùng lặp
            var conflictingAppointments = allAppointments
                .Where(a => a.UserId == userId
                    && a.AppointmentDate.Date == proposedDate.Date // Kiểm tra cùng ngày
                    && a.AppointmentDate.TimeOfDay == proposedDate.TimeOfDay // Kiểm tra cùng giờ
                    && a.DeletedTime == null) // Bỏ qua những cuộc hẹn đã bị xóa
                .ToList();

            return conflictingAppointments.Any(); // Trả về true nếu có cuộc hẹn trùng
        }



    }
}
