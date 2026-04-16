namespace Exercise03.Models
{
    public class NotificationOptions
    {
        public bool EnableEmail { get; set; } = true;
        public bool EnableSms { get; set; } = true;
        public bool EnablePush { get; set; } = true;
        public int MaxRetries { get; set; } = 3;
        public bool EnableLogging { get; set; } = true;
    }

}