namespace Exercise04.Features.ConfigurationHistory
{
    public class OptionsHistoryService
    {
        public readonly List<object> _history = new();
        public void AddSnapshot<T>(T options)
        {
            _history.Add(new OptionsSnapshot<T> {Value = options});
        }
        public IEnumerable<object> GetHistory() => _history;
    }
}