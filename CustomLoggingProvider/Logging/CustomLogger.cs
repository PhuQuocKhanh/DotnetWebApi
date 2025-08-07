using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomLoggingProvider.Models;

namespace CustomLoggingProvider.Logging
{
    public class CustomLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly string _logFilePath;
        private readonly LogLevel _minLogLevel;
        private readonly IServiceScopeFactory _scopeFactory;

        public CustomLogger(string categoryName, string logFilePath, LogLevel minLogLevel, IServiceScopeFactory scopeFactory)
        {
            _categoryName = categoryName;
            _logFilePath = logFilePath;
            _minLogLevel = minLogLevel;
            _scopeFactory = scopeFactory;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message))
                return;

            var logRecord = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {_categoryName}: {message}";
            File.AppendAllText(_logFilePath, logRecord + Environment.NewLine);

            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();
                var logEntry = new LogEntry
                {
                    Category = _categoryName,
                    Message = message,
                    EventId = eventId.Id,
                    LogLevel = logLevel.ToString(),
                    Exception = exception?.ToString(),
                    CreatedTime = DateTime.Now
                };
                dbContext.LogEntries.Add(logEntry);
                dbContext.SaveChanges();
            }
        }
    }
}