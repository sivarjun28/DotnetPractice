using System;
namespace Exerise05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a ConsoleLogger instance with minimum log level set to Info
            Logger consoleLogger = new ConsoleLogger(LogLevel.Info);

            // Log messages with different severity
            consoleLogger.Log("This is an info message.", LogLevel.Info); // Should be logged
            consoleLogger.Log("This is a debug message.", LogLevel.Debug);  // Should NOT be logged (min level is Info)
            consoleLogger.Log("This is a warning message.", LogLevel.Warning);  // Should be logged
            consoleLogger.Log("This is an error message.", LogLevel.Error);  // Should be logged

            Console.WriteLine(); // Add a blank line for separation

            // Create a FileLogger instance with minimum log level set to Warning and log file path
            Logger fileLogger = new FileLogger(LogLevel.Warning, "C:\\Logs");

            // Log messages to the file (the warning and error messages will be logged)
            fileLogger.Log("This is an info message.", LogLevel.Info); // Should NOT be logged to file (min level is Warning)
            fileLogger.Log("This is a warning message.", LogLevel.Warning); // Should be logged to file
            fileLogger.Log("This is an error message.", LogLevel.Error); // Should be logged to file

            // The log file should now have the warning and error messages
        }
    }
    /*
Create a Logger hierarchy:
1. Logger (base)
   - protected: LogLevel, OutputPath
   - protected method: FormatMessage(message)
   - public method: Log(message)
   
2. FileLogger
   - Uses protected members to write to file
   
3. ConsoleLogger
   - Uses protected members to write to console with colors
*/


    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }
    public class Logger
    {
        protected LogLevel minLevel;
        protected string outputPath;

        public Logger(LogLevel minLevel)
        {
            this.minLevel = minLevel;
            outputPath = "logs";
        }

        protected string FormatMessage(string message, LogLevel level)
        {
            return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
        }

        public virtual void Log(string message, LogLevel level)
        {
            if (level < minLevel)
                return;

            Console.WriteLine(FormatMessage(message, level));
        }

    }

    public class FileLogger : Logger
    {
        public FileLogger(LogLevel minLevel, string filePath) : base(minLevel)
        {
            outputPath = filePath;
        }

        public override void Log(string message, LogLevel level)
        {
            if (level < minLevel)
            {
                return;
            }

            string formattedMessage = FormatMessage(message, level);
            Directory.CreateDirectory(outputPath);
            string filePath = Path.Combine(outputPath, "log.txt");
            File.AppendAllText(filePath, formattedMessage + Environment.NewLine);
        }
    }

    public class ConsoleLogger : Logger
    {
        public ConsoleLogger(LogLevel minLevel) : base(minLevel)
        {

        }
        public override void Log(string message, LogLevel level)
        {
            if (level < minLevel)
                return;

            // Format the message
            string formattedMessage = FormatMessage(message, level);
            switch (level)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            // Write to console
            Console.WriteLine(formattedMessage);

            // Reset the color
            Console.ResetColor();
        }
    }
}