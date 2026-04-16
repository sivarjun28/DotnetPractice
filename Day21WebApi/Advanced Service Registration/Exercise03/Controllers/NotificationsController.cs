using Exercise03.Models;
using Exercise03.Services;
using Microsoft.AspNetCore.Mvc;

namespace Exercise03.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _emailService;
        private readonly INotificationService _smsService;
        private readonly INotificationService _pushService;

        public NotificationsController(
            [FromKeyedServices("email")] INotificationService emailService,
            [FromKeyedServices("sms")] INotificationService smsService,
            [FromKeyedServices("push")] INotificationService pushService)
        {
            _emailService = emailService;
            _smsService = smsService;
            _pushService = pushService;
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody]Notification notification)
        {
            await _emailService.SendAsync(notification);
            return Ok();
        }

        [HttpPost("sms")]
        public async Task<IActionResult> SendSms(Notification notification)
        {
            await _smsService.SendAsync(notification);
            return Ok();
        }

        [HttpPost("push")]
        public async Task<IActionResult> SendPush(Notification notification)
        {
            await _pushService.SendAsync(notification);
            return Ok();
        }
    }
}