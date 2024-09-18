using DentalBooking.Contract.Repository; // Đảm bảo đường dẫn namespace chính xác
using DentalBooking.Repository; // Đảm bảo đường dẫn namespace chính xác
using DentalBooking.Repository.Context;
using DentalBooking_Contract_Services.Interface;
using DentalBooking_Services;
using Microsoft.EntityFrameworkCore;
using Application.Converters;
using DentalBooking_Services.Service; // Đảm bảo đường dẫn namespace cho TimeOnlyConverter

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter()); // Thêm converter
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình DbContext với Lazy Loading và xác định assembly cho migrations
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseLazyLoadingProxies()
           .UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"),
           sqlOptions => sqlOptions.MigrationsAssembly("DentalBooking.Repository"));  // Chỉ định assembly chứa migrations
});

// Đăng ký các dịch vụ và triển khai của chúng
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Đảm bảo rằng UnitOfWork cài đặt IUnitOfWork
builder.Services.AddScoped<IUserService, UserService>(); // Đảm bảo rằng UserService cài đặt IUserService

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
