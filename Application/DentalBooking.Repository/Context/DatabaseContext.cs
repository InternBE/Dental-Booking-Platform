using DentalBooking.Contract.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DentalBooking.Repository.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Services> Services { get; set; }
        public DbSet<Appointment_Service> Appointment_Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<TreatmentPlans> TreatmentPlans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình kiểu dữ liệu decimal cho thuộc tính Price
            modelBuilder.Entity<Services>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18, 2)");

            // Cấu hình mối quan hệ giữa User và Clinic
            modelBuilder.Entity<User>()
                .HasOne(u => u.Clinic)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Clinic)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.TreatmentPlans)
                .WithMany(tp => tp.Appointments)
                .HasForeignKey(a => a.TreatmentPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
