using DentalBooking.ModelViews.MailModelViews;
using DentalBooking_Contract_Services.Interface;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

public class SendMailService : ISendMailService
{
    private readonly string _fromEmail;
    private readonly string _displayName;
    private readonly string _password;
    private readonly string _host;
    private readonly int _port;

    public SendMailService(IOptions<MailSettings> mailSettings)
    {
        var settings = mailSettings.Value ?? throw new ArgumentNullException(nameof(mailSettings));
        _fromEmail = settings.Mail ?? throw new ArgumentNullException(nameof(settings.Mail));
        _displayName = settings.DisplayName ?? throw new ArgumentNullException(nameof(settings.DisplayName));
        _password = settings.Password ?? throw new ArgumentNullException(nameof(settings.Password));
        _host = settings.Host ?? throw new ArgumentNullException(nameof(settings.Host));
        _port = settings.Port;
    }

    // Phương thức gửi email thông qua MailContent
    public async Task SendMail(MailContent mailContent)
    {
        // Kiểm tra xem địa chỉ email có hợp lệ không
        if (string.IsNullOrWhiteSpace(mailContent.To))
        {
            throw new ArgumentException("Địa chỉ email người nhận không hợp lệ.", nameof(mailContent.To));
        }

        // Tạo một SmtpClient mới trong phương thức gửi email
        using (var smtpClient = new SmtpClient())
        {
            try
            {
                // Kết nối đến máy chủ SMTP
                await smtpClient.ConnectAsync(_host, _port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_fromEmail, _password);

                var message = CreateEmailMessage(mailContent);
                await smtpClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Ghi lại thông tin lỗi
                throw new Exception("Đã xảy ra lỗi khi gửi email: " + ex.Message, ex);
            }
            finally
            {
                // Ngắt kết nối sau khi gửi
                await smtpClient.DisconnectAsync(true);
            }
        }
    }

    // Tạo một email message từ mailContent
    private MimeMessage CreateEmailMessage(MailContent mailContent)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_displayName, _fromEmail));
        message.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
        message.Subject = mailContent.Subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = mailContent.Body };
        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }

    // Phương thức gửi email với thông tin chuỗi
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var mailContent = new MailContent
        {
            To = email,
            Subject = subject,
            Body = htmlMessage
        };

        await SendMail(mailContent);
    }

    // Phương thức gửi email kế hoạch điều trị
    public async Task SendTreatmentPlanEmailAsync(MailContent mailContent)
    {
        await SendMail(mailContent);
    }
}
