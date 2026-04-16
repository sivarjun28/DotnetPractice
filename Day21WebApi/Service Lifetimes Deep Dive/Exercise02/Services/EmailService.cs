using System.Net.Mail;
using MimeKit;

namespace Exercise02.Services
{
    using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        try
        {
           
            using var client = new SmtpClient("smtp.yourserver.com", 587) 
            {
                Credentials = new NetworkCredential("your_username", "your_password"),
                EnableSsl = true 
            };

            
            var mailMessage = new MailMessage
            {
                From = new MailAddress("no-reply@yourdomain.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false 
            };
            mailMessage.To.Add(to);

      
            await client.SendMailAsync(mailMessage);

            _logger.LogInformation("Email sent to {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
            throw;
        }
    }
}
}