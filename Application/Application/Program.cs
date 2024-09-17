using Application.API;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Repository.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConfig(builder.Configuration);


// Cấu hình DbContext với Lazy Loading và xác định assembly cho migrations
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseLazyLoadingProxies()
           .UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"),
           sqlOptions => sqlOptions.MigrationsAssembly("DentalBooking.Repository"));  // Chỉ định assembly chứa migrations
});

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
