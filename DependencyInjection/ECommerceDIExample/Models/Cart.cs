using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; } // Foreign Key
        public User User { get; set; } // Navigation Property
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsCheckedOut { get; set; } = false;
        // Navigation property for cart items
        public ICollection<CartItem> CartItems { get; set; }
    }
}