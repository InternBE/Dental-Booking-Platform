using DentalBooking.Contract.Repository.Entity;
using Microsoft.EntityFrameworkCore;

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

            // Cấu hình các mối quan hệ
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments) // Đảm bảo rằng User có ICollection<Appointment>
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Clinic)
                .WithMany(c => c.Appointments) // Đảm bảo rằng Clinic có ICollection<Appointment>
                .HasForeignKey(a => a.ClinicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.TreatmentPlans)
                .WithMany(tp => tp.Appointments) // Đảm bảo rằng TreatmentPlans có ICollection<Appointment>
                .HasForeignKey(a => a.TreatmentPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
