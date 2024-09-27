using DentalBooking.Contract.Repository;
using DentalBooking.Repository;
using DentalBooking.Repository.Context;
using DentalBooking_Contract_Services.Interface;
using DentalBooking_Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Converters;
using DentalBooking.Services;
using Microsoft.OpenApi.Models;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking_Services.Service;
using DentalBooking.Contract.Services;
using Appointment_Service = DentalBooking_Services.Service.Appointment_Service;

var builder = WebApplication.CreateBuilder(args);

try
{
    // Add services to the container.
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        });

    builder.Services.AddEndpointsApiExplorer();

    // Cấu hình Swagger và JWT
    builder.Services.AddSwaggerGen(swagger =>
    {
        swagger.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "ASP.NET 8 Web API",
            Description = "Authentication with JWT"
        });

        swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

    // Cấu hình DbContext với Lazy Loading và xác định assembly cho migrations
    builder.Services.AddDbContext<DatabaseContext>(options =>
    {
        options.UseLazyLoadingProxies()
               .UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"), sqlOptions =>
               {
                   sqlOptions.MigrationsAssembly("DentalBooking.Repository");
                   sqlOptions.CommandTimeout(300);
               });
    });

    // Cấu hình ASP.NET Identity
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<DatabaseContext>()
        .AddDefaultTokenProviders();

    // Cấu hình JWT Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

    // Đăng ký các dịch vụ và triển khai của chúng
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IClinicService, ClinicService>();
    //builder.Services.AddScoped<IDentistService, DentistService>();
    builder.Services.AddScoped<UserManager<ApplicationUser>>();
    builder.Services.AddScoped<SignInManager<ApplicationUser>>();
    builder.Services.AddScoped<ITreatmentPlanService, TreatmentPlanService>();
    builder.Services.AddScoped<IAppointmentService, Appointment_Service>();
    builder.Services.AddScoped<IServiceServices, ServiceServices>();
    builder.Services.AddScoped<INotificationService, NotificationService>();
    builder.Services.AddScoped<IMessageService, MessageService>();
    builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

    // Build app
    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "ADMIN", "CUSTOMER", "DENTIST" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Exception during app setup: {ex.Message}");
    throw;
}
