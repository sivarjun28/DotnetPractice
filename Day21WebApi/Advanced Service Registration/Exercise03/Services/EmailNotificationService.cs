    using Exercise03.Enums;
    using Exercise03.Models;
    using System.Net.Mail;
    namespace Exercise03.Services
    {
        public class EmailNotificationService : INotificationService
        {
            public bool CanHandle(NotificationType type)
            {
                return type == NotificationType.Email;
            }

            public async Task SendAsync(Notification notification)
            {
                if (notification.Type != NotificationType.Email)
                    throw new InvalidOperationException("Cannot handle this notification type");

                using var smtp = new SmtpClient("smtp.example.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("username", "password"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("no-reply@example.com"),
                    Subject = notification.Subject,
                    Body = notification.Message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(notification.Recipient);

                await smtp.SendMailAsync(mailMessage);
            }

        }
    }