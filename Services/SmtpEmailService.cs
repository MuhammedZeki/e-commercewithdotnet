using System.Net;
using System.Net.Mail;

namespace dotnet_db.Models;


public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}


public class SmtpEmailService : IEmailService
{
    private IConfiguration _config;

    public SmtpEmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using (var client = new SmtpClient(_config["Email:Host"]))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_config["Email:Username"], _config["Email:Password"]);
            client.Port = 587;
            client.EnableSsl = true;
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Email:Username"]!),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);
            await client.SendMailAsync(mailMessage);
        }

    }
}

