﻿// <auto-generated />
using System;
using DentalBooking.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DentalBooking.Repository.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240914101227_fix-error10")]
    partial class fixerror10
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClinicUser", b =>
                {
                    b.Property<int>("ClinicId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("ClinicId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ClinicUser");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ClinicId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("DentistId")
                        .HasColumnType("int");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TreatmentPlanId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClinicId");

                    b.HasIndex("TreatmentPlanId");

                    b.HasIndex("UserId");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Appointment_Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Appointments_Service");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Clinic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClinicName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeOnly>("ClosingTime")
                        .HasColumnType("time");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("MaxPatientsPerSlot")
                        .HasColumnType("int");

                    b.Property<int>("MaxTreatmentPerSlot")
                        .HasColumnType("int");

                    b.Property<TimeOnly>("OpeningTime")
                        .HasColumnType("time");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SlotDurationMinutes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Clinic");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Services", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.TreatmentPlans", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("DentistId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("TreatmentPlans");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClinicId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastUpdatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ClinicUser", b =>
                {
                    b.HasOne("DentalBooking.Contract.Repository.Entity.Clinic", null)
                        .WithMany()
                        .HasForeignKey("ClinicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DentalBooking.Contract.Repository.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Appointment", b =>
                {
                    b.HasOne("DentalBooking.Contract.Repository.Entity.Clinic", "Clinic")
                        .WithMany("Appointments")
                        .HasForeignKey("ClinicId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("DentalBooking.Contract.Repository.Entity.TreatmentPlans", "TreatmentPlans")
                        .WithMany("Appointments")
                        .HasForeignKey("TreatmentPlanId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("DentalBooking.Contract.Repository.Entity.User", "User")
                        .WithMany("Appointments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Clinic");

                    b.Navigation("TreatmentPlans");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Appointment_Service", b =>
                {
                    b.HasOne("DentalBooking.Contract.Repository.Entity.Appointment", "Appointment")
                        .WithMany("Appointment_Services")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DentalBooking.Contract.Repository.Entity.Services", "Services")
                        .WithMany("GetAppointment_Services")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Message", b =>
                {
                    b.HasOne("DentalBooking.Contract.Repository.Entity.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.TreatmentPlans", b =>
                {
                    b.HasOne("DentalBooking.Contract.Repository.Entity.User", "User")
                        .WithMany("TreatmentPlans")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Appointment", b =>
                {
                    b.Navigation("Appointment_Services");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Clinic", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.Services", b =>
                {
                    b.Navigation("GetAppointment_Services");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.TreatmentPlans", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("DentalBooking.Contract.Repository.Entity.User", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Messages");

                    b.Navigation("TreatmentPlans");
                });
#pragma warning restore 612, 618
        }
    }
}
