namespace Exercise02.Services
{
    public interface IApplicationCache 
    {
        void Set(string key, object value);
        Object ? Get(string key);
    }
}