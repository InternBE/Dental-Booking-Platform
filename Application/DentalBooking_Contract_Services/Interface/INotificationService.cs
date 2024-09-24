using DentalBooking.ModelViews.ModelViews;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalBooking_Contract_Services.Interface
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int customerId, int doctorId, string message);
        Task<IEnumerable<NotificationModelView>> GetNotificationsAsync(int customerId);
    }
}
