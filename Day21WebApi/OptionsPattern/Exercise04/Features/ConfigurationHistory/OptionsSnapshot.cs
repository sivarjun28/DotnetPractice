namespace Exercise04.Features.ConfigurationHistory
{
    public class OptionsSnapshot<T>
    {
        public T Value {get; set;}
        public DateTime TimeStamp{get; set;} = DateTime.UtcNow;
    }
}