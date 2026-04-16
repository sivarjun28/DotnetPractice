using Exercise04.Options;
using Microsoft.Extensions.Options;

namespace Exercise04.Services
{
    public class DatabaseService1
    {
        private readonly DatabaseOptions _options;

        public DatabaseService1(IOptions<DatabaseOptions> options)
        {
            _options = options.Value;
        }
    }
}