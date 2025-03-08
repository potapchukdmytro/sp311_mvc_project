using Microsoft.AspNetCore.Identity.UI.Services;
using sp311_mvc_project.Models;
using System.Net;
using System.Net.Mail;

namespace sp311_mvc_project.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public EmailSender(IConfiguration configuration)
        {
            string host = configuration["SmtpSettings:Host"] ?? "";
            int port = int.Parse(configuration["SmtpSettings:Port"] ?? "0");
            string email = configuration["SmtpSettings:Email"] ?? "";
            string password = configuration["SmtpSettings:Password"] ?? "";

            _smtpClient = new SmtpClient(host, port);
            _smtpClient.EnableSsl = true;
            _smtpClient.Credentials = new NetworkCredential(email, password);
            _fromEmail = email;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MailMessage();
            message.From = new MailAddress(_fromEmail);
            message.To.Add(email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;
            await _smtpClient.SendMailAsync(message);
        }

        public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }
}
