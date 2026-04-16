namespace Exercise04.Services
{
    using Exercise04.Options;
    using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly EmailOptions _options;

    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;
    }

    public void SendEmail(string to, string subject, string body)
    {
        using var client = new SmtpClient(_options.SmtpHost, _options.SmtpPort)
        {
            Credentials = new NetworkCredential(_options.Username, _options.Password),
            EnableSsl = _options.EnableSsl
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_options.FromAddress),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

  
        Console.WriteLine($"Sending email to {to} using {_options.SmtpHost}");

       
    }
}
}