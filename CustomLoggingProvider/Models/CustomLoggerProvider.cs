using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomLoggingProvider.Logging;

namespace CustomLoggingProvider.Models
{
    public class CustomLoggerProvider : ILoggerProvider
    {
       // Đường dẫn file log.
        private readonly string _logFilePath;

        // Mức log tối thiểu mà provider sẽ ghi.
        private readonly LogLevel _minLogLevel;

        // IServiceScopeFactory để tạo scope DI (Dependency Injection),
        // cho phép logger resolve các service có scope, ví dụ DbContext.
        private readonly IServiceScopeFactory _scopeFactory;

        // Constructor nhận đường dẫn file log, mức log tối thiểu và scope factory.
        public CustomLoggerProvider(string logFilePath, LogLevel minimumLogLevel, IServiceScopeFactory scopeFactory)
        {
            _logFilePath = logFilePath;
            _minLogLevel = minimumLogLevel;
            _scopeFactory = scopeFactory;
        }

        // CreateLogger được gọi để tạo một ILogger cho category nhất định.
        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(categoryName, _logFilePath, _minLogLevel, _scopeFactory);
        }

        // Giải phóng tài nguyên (trong ví dụ này không cần).
        public void Dispose() { }
    }
}