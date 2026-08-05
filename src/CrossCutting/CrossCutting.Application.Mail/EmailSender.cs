using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Lazy.Crud.CrossCutting.Application.Mail
{
    public class EmailSender : IEmailSender
    {
        private readonly string _fromAddress;
        private readonly string _username;
        private readonly string _password;
        private readonly string _smtpHost;
        private readonly int _smtpPort;

        public EmailSender(IConfiguration configuration)
        {
            var section = configuration.GetSection("EmailSettings");
            _fromAddress = section["FromAddress"] ?? string.Empty;
            _username = section["Username"] ?? string.Empty;
            _password = section["Password"] ?? string.Empty;
            _smtpHost = section["SmtpHost"] ?? "smtp.gmail.com";
            _smtpPort = int.TryParse(section["SmtpPort"], out var port) ? port : 587;
        }

        public void Dispose()
        {
        }

        public async Task SendEmailAsync(string toEmailAddress, string emailSubject, string emailMessageHtml)
        {
            if (string.IsNullOrWhiteSpace(toEmailAddress)) return;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var message = new MailMessage();
            message.To.Add(toEmailAddress);
            message.Subject = emailSubject;
            message.Body = emailMessageHtml;
            message.IsBodyHtml = true;
            message.From = new MailAddress(_fromAddress);
                
            using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(_username, _password);
                try
                {
                    await smtpClient.SendMailAsync(message);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
    }

    public interface IEmailSender : IDisposable
    {
        Task SendEmailAsync(string toEmailAddress, string emailSubject, string emailMessageHtml);
    }
}

