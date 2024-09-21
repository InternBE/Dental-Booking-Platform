using DentalBooking.Contract.Repository;
using DentalBooking.Repository;
using DentalBooking.Repository.Context;
using DentalBooking_Contract_Services.Interface;
using DentalBooking_Services;
using Microsoft.EntityFrameworkCore;
using Application.Converters;
using DentalBooking.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    // Thiết lập thông tin cho Swagger UI
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 8 Web API",
        Description = "Authentication with JWT"
    });

    // Cấu hình bảo mật JWT cho Swagger
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",                     // Tên trường tiêu đề
        Type = SecuritySchemeType.ApiKey,           // Loại bảo mật là ApiKey
        Scheme = "Bearer",                          // Loại mã bảo mật là Bearer token
        BearerFormat = "JWT",                       // Định dạng của mã là JWT
        In = ParameterLocation.Header               // Thêm mã này vào phần tiêu đề của yêu cầu HTTP
    });

    // Thêm yêu cầu bảo mật để tất cả các API endpoint phải được xác thực
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
           .UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"),
           sqlOptions => sqlOptions.MigrationsAssembly("DentalBooking.Repository"));
});

// Đăng ký các dịch vụ và triển khai của chúng
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
// Đăng ký IClinicService
builder.Services.AddScoped<IClinicService, ClinicService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
