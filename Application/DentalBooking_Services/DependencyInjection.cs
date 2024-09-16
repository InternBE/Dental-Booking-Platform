using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DentalBooking.Contract.Repository.Interface;
using DentalBooking.Repository.UOW;

namespace DentalBooking.Services
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories();
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
