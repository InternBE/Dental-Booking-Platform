using DentalBooking.Contract.Repository;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Services;
using DentalBooking.Core.Utils;
using DentalBooking.ModelViews.AppointmentModelViews;
using DentalBooking_Contract_Services.Interface;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            // Kiểm tra tính hợp lệ của model
            if (model == null || model.UserId <= 0 || model.ClinicId <= 0 || model.TreatmentPlanId <= 0)
            {
                throw new ArgumentException("Dữ liệu đầu vào không hợp lệ. Vui lòng kiểm tra các trường thông tin.");
            }

            // Kiểm tra ngày hẹn không được trong quá khứ
            if (model.AppointmentDate <= DateTime.Now)
            {
                throw new InvalidOperationException("Ngày hẹn phải là ngày trong tương lai.");
            }

            // Tạo đối tượng Appointment mới
            var appointmentEntity = new Appointment
            {
                UserId = model.UserId,
                ClinicId = model.ClinicId,
                TreatmentPlanId = model.TreatmentPlanId,
                AppointmentDate = model.AppointmentDate,
                Status = model.Status
            };

            // Thêm cuộc hẹn vào database
            var repository = _unitOfWork.GetRepository<Appointment>();
            await repository.InsertAsync(appointmentEntity);
            await _unitOfWork.SaveAsync();

            // Trả về thông tin cuộc hẹn vừa tạo
            return new AppointmentResponeModelViews
            {
                AppointmentDate = appointmentEntity.AppointmentDate,
                Status = appointmentEntity.Status,
                UserId = appointmentEntity.UserId,
                ClinicId = appointmentEntity.ClinicId,
                TreatmentPlanId = appointmentEntity.TreatmentPlanId
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
        //Điều trị định kỳ
        public async Task<List<AppointmentResponeModelViews>> BookPeriodicAppointmentsAsync(AppointmentRequestModelView model, int months)
        {
            // Kiểm tra dữ liệu đầu vào
            if (model == null || model.UserId <= 0 || model.ClinicId <= 0 || model.TreatmentPlanId <= 0)
            {
                throw new ArgumentException("Dữ liệu đầu vào không hợp lệ. Vui lòng kiểm tra các trường thông tin.");
            }

            // Kiểm tra số tháng định kỳ phải lớn hơn 0
            if (months <= 0)
            {
                throw new ArgumentException("Số tháng phải lớn hơn 0.");
            }

            // Kiểm tra ngày hẹn đầu tiên phải là ngày trong tương lai
            if (model.AppointmentDate <= DateTime.Now)
            {
                throw new InvalidOperationException("Ngày hẹn phải là ngày trong tương lai.");
            }

            // Giới hạn thời gian cho việc đặt lịch định kỳ (ví dụ chỉ trong vòng 12 tháng)
            if (months > 12)
            {
                throw new InvalidOperationException("Chỉ được phép đặt lịch trong vòng 12 tháng.");
            }

            // Lấy repository và tạo danh sách phản hồi
            var repository = _unitOfWork.GetRepository<Appointment>();
            var responseAppointments = new List<AppointmentResponeModelViews>();

            // Lấy danh sách tất cả cuộc hẹn của nha sĩ trong khoảng thời gian cần kiểm tra
            var dentistAppointments = await repository.Entities
                .Where(a => a.ClinicId == model.ClinicId && a.AppointmentDate >= model.AppointmentDate)
                .ToListAsync();

            // Lấy danh sách cuộc hẹn hiện tại của người dùng
            var userAppointments = await repository.Entities
                .Where(a => a.UserId == model.UserId && a.AppointmentDate >= model.AppointmentDate)
                .ToListAsync();

            // Vòng lặp để đặt lịch định kỳ theo tháng
            for (int i = 0; i < months; i++)
            {
                var appointmentDate = model.AppointmentDate.AddMonths(i);

                // Kiểm tra nếu nha sĩ có lịch trùng
                if (dentistAppointments.Any(a => a.AppointmentDate.Date == appointmentDate.Date))
                {
                    throw new InvalidOperationException($"Nha sĩ đã có lịch hẹn vào ngày {appointmentDate.ToShortDateString()}");
                }

                // Kiểm tra nếu người dùng có lịch trùng
                if (userAppointments.Any(a => a.AppointmentDate.Date == appointmentDate.Date))
                {
                    throw new InvalidOperationException($"Người dùng đã có lịch hẹn vào ngày {appointmentDate.ToShortDateString()}");
                }

                // Tạo và lưu cuộc hẹn mới
                var appointmentEntity = new Appointment
                {
                    UserId = model.UserId,
                    ClinicId = model.ClinicId,
                    TreatmentPlanId = model.TreatmentPlanId,
                    AppointmentDate = appointmentDate,
                    Status = model.Status
                };

                await repository.InsertAsync(appointmentEntity);

                // Thêm cuộc hẹn vào danh sách phản hồi
                responseAppointments.Add(new AppointmentResponeModelViews
                {
                    AppointmentDate = appointmentEntity.AppointmentDate,
                    Status = appointmentEntity.Status,
                    UserId = appointmentEntity.UserId,
                    ClinicId = appointmentEntity.ClinicId,
                    TreatmentPlanId = appointmentEntity.TreatmentPlanId
                });
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _unitOfWork.SaveAsync();

            return responseAppointments;
        }


        public async Task<IEnumerable<AppointmentResponeModelViews>> AllAppointmentsByUserIdAsync(int UserId)
        {
            var repository = _unitOfWork.GetRepository<Appointment>();

            // Chờ để nhận danh sách các cuộc hẹn
            IEnumerable<Appointment> lAppointment = await repository.FindAsync(x => x.UserId == UserId);

            return lAppointment.Select(appointment => new AppointmentResponeModelViews
            {
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                UserId = appointment.UserId,
                ClinicId = appointment.ClinicId,
                TreatmentPlanId = appointment.TreatmentPlanId
            }
            );



        }

        public async Task<IEnumerable<AppointmentResponeModelViews>> AlertAppointmentDayBefore(int userid, bool isAlert)
        {
            var repository = _unitOfWork.GetRepository<Appointment>();
            IEnumerable<AppointmentResponeModelViews> response = await AllAppointmentsByUserIdAsync(userid);
            if (response.IsNullOrEmpty())
            {
                return null;
            }
            if (!isAlert)
            {
                return null;
            }
            DateOnly Today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly Tomorrow = Today.AddDays(1);

            var haveAppointmentDaytBefore = response.Where(appointment => DateOnly.FromDateTime(appointment.AppointmentDate).Equals(Tomorrow));
            if (haveAppointmentDaytBefore == null)
            {
                return null;
            }
            return haveAppointmentDaytBefore;
        }
    }
}
