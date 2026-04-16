namespace Exercise02.Services
{
    public interface IRequestContext
    {
        string RequestId{get;}
        string ? UserId{get;}
        DateTime RequestStartTime{get;}
        void SetUserId(string userId);
    }
}