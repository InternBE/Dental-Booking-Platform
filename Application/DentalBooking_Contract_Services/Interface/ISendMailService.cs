using DentalBooking.ModelViews.MailModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking_Contract_Services.Interface
{
    public interface ISendMailService
    {
        Task SendMail(MailContent mailContent);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendTreatmentPlanEmailAsync(MailContent mailContent);
    }
}
