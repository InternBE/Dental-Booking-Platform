using DentalBooking.Contract.Repository.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DentalBooking.Repository.Context
{
    // Kế thừa từ IdentityDbContext thay vì DbContext
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        // Các DbSet cho các thực thể khác
        public DbSet<Services> Services { get; set; }
        public DbSet<Appointment_Service> Appointment_Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<TreatmentPlans> TreatmentPlans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Quan trọng: Gọi base.OnModelCreating để Identity hoạt động đúng

            // Cấu hình khác cho các thực thể của bạn
            modelBuilder.Entity<Services>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18, 2)");

            // Cấu hình các mối quan hệ
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
        }
    }
}
