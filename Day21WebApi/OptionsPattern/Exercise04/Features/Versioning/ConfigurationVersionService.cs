namespace Exercise04.Features.Versioning
{
    public class ConfigurationVersionService
    {
        private int _version = 1;

        public int GetVersion() => _version;
        public void IncrementVersion()
        {
            _version++;
        }
    }
}