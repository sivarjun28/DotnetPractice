using Exercise04.Features.Audit;
using Exercise04.Features.ConfigurationHistory;
using Exercise04.Features.Versioning;
using Exercise04.Options;
using Microsoft.Extensions.Options;

namespace Exercise04.Services
{
    public class DatabaseService3
    {
        private readonly IOptionsMonitor<DatabaseOptions> _options;

        public DatabaseService3(IOptionsMonitor<DatabaseOptions> options,
                                OptionsHistoryService history,
                                ConfigurationAuditService audit,
                                ConfigurationVersionService version)
        {
            _options = options;
            _options.OnChange(newOptions =>
            {
                history.AddSnapshot(newOptions);
                audit.LogChange("DatabaseOptions updated");
                version.IncrementVersion();
            });
        }
        public string GetConnection()
        {
            return _options.CurrentValue.ConnectionString;
        }
    }
}