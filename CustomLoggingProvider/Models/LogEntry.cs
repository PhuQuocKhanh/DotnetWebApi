using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomLoggingProvider.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string? Category { get; set; } 
        public string? Message { get; set; }
        public string LogLevel { get; set; }
        public int? EventId { get; set; }
        public string? Exception { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}