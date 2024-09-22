using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DentalBooking.Contract.Repository.Entity;

namespace DentalBooking.Repository.Context
{
    public class DataSeeder
    {
        public static void SeedData(DatabaseContext context, string jsonFilePath)
        {
            var jsonData = File.ReadAllText(jsonFilePath);
            var data = JsonSerializer.Deserialize<ClinicData>(jsonData);

            using var transaction = context.Database.BeginTransaction();

                // 1. Chèn dữ liệu vào bảng Clinics trước
                if (data.Clinics != null && data.Clinics.Any())
                {
                    foreach (var clinic in data.Clinics)
                    {
                        context.Clinics.Add(new Clinic
                        {
                            ClinicName = clinic.ClinicName,
                            Address = clinic.Address,
                            PhoneNumber = clinic.PhoneNumber,
                            OpeningTime = TimeOnly.Parse(clinic.OpeningTime.ToString()),
                            ClosingTime = TimeOnly.Parse(clinic.ClosingTime.ToString()),
                            SlotDurationMinutes = clinic.SlotDurationMinutes,
                            MaxPatientsPerSlot = clinic.MaxPatientsPerSlot,
                            MaxTreatmentPerSlot = clinic.MaxTreatmentPerSlot,
                            CreatedTime = clinic.CreatedTime,
                            LastUpdatedTime = clinic.LastUpdatedTime
                        });
                    }
                    context.SaveChanges();
                }

                // 2. Chèn dữ liệu vào bảng Users (sau khi Clinics đã được chèn)
                if (data.Users != null && data.Users.Any())
                {
                    foreach (var user in data.Users)
                    {
                        context.Users.Add(new User
                        {
                            FullName = user.FullName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            ClinicId = user.ClinicId,  // ClinicId phải tồn tại trong bảng Clinics
                            CreatedTime = user.CreatedTime,
                            LastUpdatedTime = user.LastUpdatedTime
                        });
                    }
                    context.SaveChanges();
                }

                // 3. Chèn dữ liệu vào bảng Services
                if (data.Services != null && data.Services.Any())
                {
                    foreach (var service in data.Services)
                    {
                        context.Services.Add(new Services
                        {
                            ServiceName = service.ServiceName,
                            Description = service.Description,
                            Price = service.Price,
                            CreatedTime = service.CreatedTime,
                            LastUpdatedTime = service.LastUpdatedTime
                        });
                    }
                    context.SaveChanges();
                }

                // 4. Chèn dữ liệu vào bảng TreatmentPlans (sau khi Users và Clinics đã được chèn)
                if (data.TreatmentPlans != null && data.TreatmentPlans.Any())
                {
                    foreach (var plan in data.TreatmentPlans)
                    {
                        context.TreatmentPlans.Add(new TreatmentPlans
                        {
                            Description = plan.Description,
                            StartDate = plan.StartDate,
                            EndDate = plan.EndDate,
                            CustomerId = plan.CustomerId,   // CustomerId phải tồn tại
                            CreatedTime = plan.CreatedTime,
                            LastUpdatedTime = plan.LastUpdatedTime
                        });
                    }
                    context.SaveChanges();
                }

                // 5. Chèn dữ liệu vào bảng Appointments (sau khi TreatmentPlans, Users, Clinics đã được chèn)
                if (data.Appointments != null && data.Appointments.Any())
                {
                    foreach (var appointment in data.Appointments)
                    {
                        context.Appointments.Add(new Appointment
                        {
                            AppointmentDate = appointment.AppointmentDate,
                            Status = appointment.Status,
                            UserId = appointment.UserId,                  // UserId phải tồn tại
                            ClinicId = appointment.ClinicId,              // ClinicId phải tồn tại
                            TreatmentPlanId = appointment.TreatmentPlanId, // TreatmentPlanId phải tồn tại
                            CreatedTime = appointment.CreatedTime,
                            LastUpdatedTime = appointment.LastUpdatedTime
                        });
                    }
                    context.SaveChanges();
                }

                // 6. Chèn dữ liệu vào bảng Appointments_Services (sau khi Appointments và Services đã được chèn)
                if (data.Appointments_Services != null && data.Appointments_Services.Any())
                {
                    foreach (var appointmentService in data.Appointments_Services)
                    {
                        // Đảm bảo appointmentService là kiểu Appointment_Service
                        context.Appointment_Services.Add(new Appointment_Service
                        {
                            AppointmentId = appointmentService.AppointmentId,  // Kiểm tra thuộc tính này
                            ServiceId = appointmentService.ServiceId,          // Kiểm tra thuộc tính này
                            CreatedTime = appointmentService.CreatedTime,
                            LastUpdatedTime = appointmentService.LastUpdatedTime
                        });
                    }
                    context.SaveChanges();
                }


                // Hoàn thành giao dịch
                transaction.Commit();
            }

        }
    public class ClinicData
    {
        public List<Clinic> Clinics { get; set; }
        public List<User> Users { get; set; }
        public List<Services> Services { get; set; }
        public List<TreatmentPlans> TreatmentPlans { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Appointment_Service> Appointments_Services { get; set; }
    }
}
    

