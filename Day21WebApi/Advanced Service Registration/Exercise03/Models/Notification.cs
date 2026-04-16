using Exercise03.Enums;

namespace Exercise03.Models
{
    public class Notification
    {
        public NotificationType Type { get; set; }
        public string Recipient { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}