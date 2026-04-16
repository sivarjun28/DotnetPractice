namespace Exercise02.Services
{
    public class RequestContext : IRequestContext
    {
        public string RequestId { get; }
        public string? UserId { get; private set; }
        public DateTime RequestStartTime { get; }
        public RequestContext()
        {
            RequestId = Guid.NewGuid().ToString();
            RequestStartTime = DateTime.UtcNow;
        }

        public void SetUserId(string userId)
        {
            UserId = userId;
        }
    }
}