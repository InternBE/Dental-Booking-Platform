using DentalBooking.Contract.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DentalBooking.Repository.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Services> Services { get; set; }
        public DbSet<Appointment_Service> Appointments_Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<TreatmentPlans> TreatmentPlans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Clinic)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ClinicId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.TreatmentPlans)
                .WithMany(t => t.Appointments)
                .HasForeignKey(a => a.TreatmentPlanId).OnDelete(DeleteBehavior.NoAction);
        }

        public void SeedData()
        {
            if (true)
            {

                // Trong phương thức SeedData
                //var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Application", "data.json");
                //var jsonData = File.ReadAllText(jsonFilePath);
                var jsonData = File.ReadAllText("C://Users//Admin//Desktop//DenTal-Booking-Platform//Application//Application//data.json");


                var data = JsonConvert.DeserializeObject<SeedData>(jsonData);

                Clinics.AddRange(data.Clinics);
                Users.AddRange(data.Users);
                Services.AddRange(data.Services);
                Appointments.AddRange(data.Appointments);
                TreatmentPlans.AddRange(data.TreatmentPlans);
                Messages.AddRange(data.Messages);

                SaveChanges();
            }
        }
    }
    public class SeedData
    {
        public List<Clinic> Clinics { get; set; }
        public List<User> Users { get; set; }
        public List<Services> Services { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<TreatmentPlans> TreatmentPlans { get; set; }
        public List<Message> Messages { get; set; }
    }
}
