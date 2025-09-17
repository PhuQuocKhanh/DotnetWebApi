using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.DTOs
{
    public class CartResponseDTO
    {
        // ID of the cart.
        public int CartId { get; set; }
        // ID of the user who owns the cart.
        public int UserId { get; set; }
        // Indicates whether the cart has been checked out.
        public bool IsCheckedOut { get; set; }
        // Timestamp when the cart was created.
        public DateTime CreatedAt { get; set; }
        // Timestamp when the cart was last updated.
        public DateTime UpdatedAt { get; set; }
        // Collection of items in the cart.
        public IEnumerable<CartItemResponseDTO>? Items { get; set; }
        // Total amount before discounts.
        public decimal TotalAmount { get; set; }
        // Discount rate applied based on user type.
        public decimal DiscountRate { get; set; }
        // Total amount after applying discounts.
        public decimal DiscountedTotal { get; set; }
    }
}