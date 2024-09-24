using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository;
using DentalBooking_Contract_Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalBooking_Services.Service
{
    public class DentistService : IDentistService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DentistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            var appointmentRepository = _unitOfWork.GetRepository<Appointment>();
            // Sử dụng ToList() để chuyển đổi IEnumerable<Appointment> thành List<Appointment>
            return (await appointmentRepository.GetAllAsync()).ToList();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            var appointmentRepository = _unitOfWork.GetRepository<Appointment>();
            var appointment = await appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy lịch hẹn với ID {id}.");
            }
            return appointment;
        }

        public async Task<Appointment> ScheduleAppointmentAsync(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment), "Thông tin lịch hẹn là bắt buộc.");
            }

            var appointmentRepository = _unitOfWork.GetRepository<Appointment>();
            appointment.Status = "Scheduled";
            await appointmentRepository.InsertAsync(appointment);
            await _unitOfWork.SaveAsync();
            return appointment;
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            var appointmentRepository = _unitOfWork.GetRepository<Appointment>();
            var existingAppointment = await appointmentRepository.GetByIdAsync(appointment.Id);

            if (existingAppointment == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy lịch hẹn với ID {appointment.Id}.");
            }

            existingAppointment.AppointmentDate = appointment.AppointmentDate;
            existingAppointment.Status = appointment.Status;
            existingAppointment.TreatmentPlanId = appointment.TreatmentPlanId;
            existingAppointment.ClinicId = appointment.ClinicId;

            appointmentRepository.Update(existingAppointment);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointmentRepository = _unitOfWork.GetRepository<Appointment>();
            var appointment = await appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy lịch hẹn với ID {id}.");
            }

            appointmentRepository.Delete(appointment);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
