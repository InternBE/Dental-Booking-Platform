using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking_Contract_Services.Interface;
using DentalBooking.Repository.Context;
using DentalBooking.Services;
using DentalBooking_Services.Services;


namespace Application.API
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            
            services.AddInfrastructure(configuration);
            services.AddServices();
        }
        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("MyCnn"));
            });
        }

       
        public static void AddServices(this IServiceCollection services)
        {
            services
                //.AddScoped<IUserService, UserService>()
                .AddScoped<IUserService, UserService>();
        }
    }
}
