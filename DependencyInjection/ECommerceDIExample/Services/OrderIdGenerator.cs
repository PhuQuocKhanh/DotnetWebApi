using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.Services
{
    // Defines the contract for generating new order IDs.
    public interface IOrderIdGenerator
    {
        string GenerateOrderId();
    }
    // Generates order IDs in a transient fashion (new instance for each request).
    // Useful for scenarios where each usage should be fully isolated.
    public class OrderIdGenerator : IOrderIdGenerator
    {
        private readonly ILoggerService _logger;
        public OrderIdGenerator(ILoggerService logger)
        {
            _logger = logger;
            _logger.Log("Creating OrderIdGenerator (Transient).");
        }
        // Generate a "realistic" order ID, e.g., ORD202501231234567.
        // This uses the current datetime plus a short random suffix.
        public string GenerateOrderId()
        {
            // Example format: ORD-[Year][Month][Day][HHmmss]-[random 4 digits]
            var now = DateTime.Now;
            var randomSuffix = new Random().Next(1000, 9999).ToString();
            var orderId = $"ORD-{now:yyyyMMddHHmmss}-{randomSuffix}";
            return orderId;
        }
    }
}