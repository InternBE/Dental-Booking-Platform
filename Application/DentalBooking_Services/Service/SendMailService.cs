using DentalBooking.ModelViews.MailModelViews;
using DentalBooking_Contract_Services.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DentalBooking_Services.Service
{
    public class SendMailService : ISendMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<SendMailService> _logger;

        public SendMailService(IOptions<MailSettings> mailSettings, ILogger<SendMailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder { HtmlBody = mailContent.Body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            try
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendMail(new MailContent
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }

        public async Task SendTreatmentPlanEmailAsync(MailContent mailContent)
        {
            await SendMail(mailContent);
        }
    }
}
