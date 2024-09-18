using DentalBooking.Contract.Repository;
using DentalBooking.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



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
