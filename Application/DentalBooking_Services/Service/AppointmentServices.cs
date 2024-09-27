using DentalBooking.Contract.Repository;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.ModelViews.AppointmentModelViews;
using DentalBooking_Contract_Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DentalBooking_Services.Service
{
    public class Appointment_Service : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentRepository _appointmentRepository;

        public Appointment_Service(IUnitOfWork unitOfWork, IAppointmentRepository appointmentRepository)
        {
            _unitOfWork = unitOfWork;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<Appointment>> GetPaginatedAppointmentsAsync(int pageNumber, int pageSize)
        {
            return await _appointmentRepository.GetPaginatedAppointmentsAsync(pageNumber, pageSize);
        }

        public async Task<double> GetTotalAppointmentsCountAsync()
        {
            return await _appointmentRepository.GetTotalAppointmentsCountAsync();
        }

        public async Task<AppointmentResponeModelViews> CreateAppointmentAsync(AppointmentRequestModelView model)
        {
            if (model == null || model.UserId <= 0 || model.ClinicId <= 0 || model.TreatmentPlanId <= 0)
            {
                throw new ArgumentException("Dữ liệu đầu vào không hợp lệ. Vui lòng kiểm tra các trường thông tin.");
            }

            if (model.AppointmentDate <= DateTime.Now)
            {
                throw new InvalidOperationException("Ngày hẹn phải là ngày trong tương lai.");
            }

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
            var appointments = await repository.GetAllAsync();
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

            if (appointment == null) return false;

            if (appointment.DeletedTime == null)
            {
                appointment.DeletedTime = DateTimeOffset.Now;
                appointment.DeletedBy = "Current User"; // Thay thế bằng thông tin người dùng hiện tại nếu có
                repository.Update(appointment);
            }
            else
            {
                repository.Delete(id);
            }

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<List<AppointmentResponeModelViews>> BookPeriodicAppointmentsAsync(AppointmentRequestModelView model, int months)
        {
            if (model == null || model.UserId <= 0 || model.ClinicId <= 0 || model.TreatmentPlanId <= 0)
            {
                throw new ArgumentException("Dữ liệu đầu vào không hợp lệ. Vui lòng kiểm tra các trường thông tin.");
            }

            if (months <= 0)
            {
                throw new ArgumentException("Số tháng phải lớn hơn 0.");
            }

            if (model.AppointmentDate <= DateTime.Now)
            {
                throw new InvalidOperationException("Ngày hẹn phải là ngày trong tương lai.");
            }

            if (months > 12)
            {
                throw new InvalidOperationException("Chỉ được phép đặt lịch trong vòng 12 tháng.");
            }

            var repository = _unitOfWork.GetRepository<Appointment>();
            var responseAppointments = new List<AppointmentResponeModelViews>();

            var dentistAppointments = await repository.Entities
                .Where(a => a.ClinicId == model.ClinicId && a.AppointmentDate >= model.AppointmentDate)
                .ToListAsync();

            var userAppointments = await repository.Entities
                .Where(a => a.UserId == model.UserId && a.AppointmentDate >= model.AppointmentDate)
                .ToListAsync();

            for (int i = 0; i < months; i++)
            {
                var appointmentDate = model.AppointmentDate.AddMonths(i);

                if (dentistAppointments.Any(a => a.AppointmentDate.Date == appointmentDate.Date))
                {
                    throw new InvalidOperationException($"Nha sĩ đã có lịch hẹn vào ngày {appointmentDate.ToShortDateString()}");
                }

                if (userAppointments.Any(a => a.AppointmentDate.Date == appointmentDate.Date))
                {
                    throw new InvalidOperationException($"Người dùng đã có lịch hẹn vào ngày {appointmentDate.ToShortDateString()}");
                }

                var appointmentEntity = new Appointment
                {
                    UserId = model.UserId,
                    ClinicId = model.ClinicId,
                    TreatmentPlanId = model.TreatmentPlanId,
                    AppointmentDate = appointmentDate,
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
            var appointments = await repository.FindAsync(x => x.UserId == UserId);

            return appointments.Select(appointment => new AppointmentResponeModelViews
            {
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                UserId = appointment.UserId,
                ClinicId = appointment.ClinicId,
                TreatmentPlanId = appointment.TreatmentPlanId
            });
        }

        public async Task<IEnumerable<AppointmentResponeModelViews>> AlertAppointmentDayAfter(int userId, bool isAlert)
        {
            var repository = _unitOfWork.GetRepository<Appointment>();
            var response = await AllAppointmentsByUserIdAsync(userId);
            if (response == null || !isAlert)
            {
                return null;
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly tomorrow = today.AddDays(1);

            return response.Where(appointment => DateOnly.FromDateTime(appointment.AppointmentDate) == tomorrow);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDentistIdAsync(int dentistId)
        {
            return await _appointmentRepository.GetAppointmentsByDentistIdAsync(dentistId);
        }

        public Task<List<AppointmentResponeModelViews>> GetWeeklyScheduleForDentist(int dentistId)
        {
            throw new NotImplementedException();
        }
    }
}
