namespace Exercise04.Options
{
    public class DatabaseOptions
    {
         public const string Section = "Database";
        public string ConnectionString { get; set; } = " ";
        public int MaxRetries { get; set; } = 3;
        public int CommandTimeOut { get; set; } = 30;
        public bool EnableSensitiveDataLogging { get; set; } = false;
    }
}