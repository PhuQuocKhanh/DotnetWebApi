using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.Services
{
    public interface IGlobalDiscountService
    {
        // Returns a discount rate (0.0 to 1.0) for the specified user type.
        decimal GetDiscountRate(string? userType = null);
        // Sets a default discount rate for fallback or general use.
        void SetDefaultDiscountRate(decimal newRate);
    }

    // Manages application-wide discounts.
    // Registered as a Singleton, so one instance is shared across the entire application lifetime.
    public class GlobalDiscountService : IGlobalDiscountService
    {
        private readonly ILoggerService _logger;
        // Example: We keep a default discount and specialized discount rules by user role.
        private decimal _defaultDiscountRate;
        private Dictionary<string, decimal> _userTypeBasedDiscounts;
        public GlobalDiscountService(ILoggerService logger)
        {
            _logger = logger;
            _logger.Log("Creating GlobalDiscountService (Singleton).");
            // Default discount
            _defaultDiscountRate = 0.05m;
            // Simulate role-based discount
            _userTypeBasedDiscounts = new Dictionary<string, decimal>
            {
                { "VIP", 0.10m },
                { "Premium", 0.08m },
                { "Standard", 0.05m },
                { "Guest", 0.02m }
            };
        }
        // Returns the discount rate for a given user role.
        public decimal GetDiscountRate(string? userType = null)
        {
            if (!string.IsNullOrEmpty(userType) && _userTypeBasedDiscounts.ContainsKey(userType))
            {
                return _userTypeBasedDiscounts[userType];
            }
            return _defaultDiscountRate; // fallback if no User Type is found
        }
        // Updates the default discount rate for the entire application.
        public void SetDefaultDiscountRate(decimal newRate)
        {
            _defaultDiscountRate = newRate;
            //_logger.Log($"Global discount updated to {newRate:P2}");
        }
    }
}