using Exercise04.Options;
using Microsoft.Extensions.Options;

namespace Exercise04.Services
{
    public class DatabaseService2
    {
        private readonly IOptionsSnapshot<DatabaseOptions> _options;
        public DatabaseService2(IOptionsSnapshot<DatabaseOptions> options)
        {
            _options = options;
        }
        public string GetConnection()
        {
            return _options.Value.ConnectionString;
        }

    }
}