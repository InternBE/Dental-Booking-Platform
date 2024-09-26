using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository;
using DentalBooking.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DentalBooking.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly DatabaseContext _context;

        public AppointmentRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        // Thực thi phương thức lấy lịch hẹn của dentist
        public async Task<IEnumerable<Appointment>> GetAppointmentsByDentistIdAsync(int dentistId)
        {
            return await _context.Appointments
                .Where(a => a.DentistId == dentistId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Appointment>> GetPaginatedAppointmentsAsync(int pageNumber, int pageSize)
        {
            return await _context.Appointments
                                 .Skip((pageNumber - 1) * pageSize)  // Bỏ qua các bản ghi trước đó
                                 .Take(pageSize)                     // Lấy một số lượng bản ghi cụ thể
                                 .ToListAsync();
        }

        // Đếm tổng số cuộc hẹn
        public async Task<int> GetTotalAppointmentsCountAsync()
        {
            return await _context.Appointments.CountAsync(); // Đếm tổng số bản ghi
        }


        Task<IEnumerable<Appointment>> IAppointmentRepository.FindAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }

}
