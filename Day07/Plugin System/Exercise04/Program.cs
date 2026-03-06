using System;
namespace Exercise04
{
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }
        string Author { get; }

        void Initialize();
        void Execute();
        void Cleanup();
    }

    public interface IConfigurable
    {
        void LoadConfig(Dictionary<string, string> settings);
        Dictionary<string, string> SaveConfig();
    }

    public interface ILoggable
    {
        void LogInfo(string message);
        void LogError(string message);
    }

    // TODO: Implement plugins

    public class DataExportPlugin : IPlugin, IConfigurable
    {
        public string Name => "Data Exporter";
        public string Version => "1.0.0";
        public string Author => "VSC";

        private string exportFormat = "CSV";

        public void Initialize()
        {
            Console.WriteLine($"{Name} v{Version} initialized");
        }

        public void Execute()
        {
            Console.WriteLine($"Exporting data as {exportFormat}");
            // Export logic
        }

        public void Cleanup()
        {
            Console.WriteLine($"{Name} cleaned up");
        }

        public void LoadConfig(Dictionary<string, string> settings)
        {
            if (settings.ContainsKey("format"))
            {
                exportFormat = settings["format"];
            }
        }

        public Dictionary<string, string> SaveConfig()
        {
            return new Dictionary<string, string>
            {
                ["format"] = exportFormat
            };
        }
    }

    // TODO: Implement EmailPlugin
    public class EmailPlugin : IPlugin, IConfigurable, ILoggable
    {
        public string Name => "Email sender";
        public string Version => "1.0.0";
        public string Author => "VSC";

        private String smtpServer = "smtp.example.com";
        private string recipient = "user@example.com";
        public void Initialize()
        {
            System.Console.WriteLine($"{Name} v{Version} intialized");
        }

        public void Execute()
        {
            System.Console.WriteLine($"sending mail to a {recipient} via {smtpServer}");
        }
        public void Cleanup()
        {
            System.Console.WriteLine($"{Name} cleaned up");
        }

        public void LoadConfig(Dictionary<string,string> settings)
        {
            if (settings.ContainsKey("smtpServer"))
            {
                smtpServer = settings["smtpServer"];
            }
            if (settings.ContainsKey("recipient"))
            {
                recipient = settings["recipient"];
            }
        }

        public Dictionary<string, string> SaveConfig()
        {
            return new Dictionary<string, string>
            {
                ["smtpserver"] = smtpServer,
                ["recipient"] = recipient
            };
        }

        public void LogInfo(string message)
        {
            System.Console.WriteLine($"INFO: {message}");
        }
        public void LogError(string message)
        {
            System.Console.WriteLine($"ERROR: {message}");
        }
    }

    // TODO: Implement ReportGeneratorPlugin
    public class ReportGeneratorPlugin : IPlugin, ILoggable
    {
        public string Name => "Report Generatir";
        public string Version => "1.0.0";
        public string Author => "VSC";

        public void Initialize()
        {
            System.Console.WriteLine($"{Name} v{Version} initialized");
        }
        public void Execute()
        {
            System.Console.WriteLine($"Generating Report......");
        }
        public void Cleanup()
        {
            System.Console.WriteLine($"{Name} cleaned up");
        }
        public void LogInfo(string message)
        {
            System.Console.WriteLine($"INFO: {message}");
        }
        public void LogError(string message)
        {
            System.Console.WriteLine($"ERROR: {message}");
        }
    }

    // Plugin Manager
    public class PluginManager
    {
        private List<IPlugin> plugins = new();

        public void LoadPlugin(IPlugin plugin)
        {
            plugins.Add(plugin);
            plugin.Initialize();

            // Configure if possible
            if (plugin is IConfigurable configurable)
            {
                var config = new Dictionary<string, string>
                {
                    ["format"] = "JSON"
                };
                configurable.LoadConfig(config);
            }
        }

        public void ExecuteAll()
        {
            foreach (var plugin in plugins)
            {
                plugin.Execute();
            }
        }

        public void UnloadAll()
        {
            foreach (var plugin in plugins)
            {
                plugin.Cleanup();
            }
            plugins.Clear();
        }

        public T? GetPlugin<T>() where T : IPlugin
        {
            return plugins.OfType<T>().FirstOrDefault();
        }
    }

    internal class Program()
    {
        static void Main(string[] args)
        {
            PluginManager manager = new();
            manager.LoadPlugin(new DataExportPlugin());
            manager.LoadPlugin(new EmailPlugin());
            manager.LoadPlugin(new ReportGeneratorPlugin());

            manager.ExecuteAll();
            manager.UnloadAll();
        }
    }

}
