using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.Services
{
    // A singleton service responsible for logging messages to a text file.
    public class LoggerService : ILoggerService
    {
        private readonly string _logFilePath;
        
        public LoggerService()
        {
            // Log file path (ensure "logs" folder exists or adjust as needed)
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "app_log.txt");
            // Create logs directory if it doesn't exist
            var directory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        // Logs a message with a timestamp to the log file.
        public void Log(string message)
        {
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logMessage);
        }
    }
}