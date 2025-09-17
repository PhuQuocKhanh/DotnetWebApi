using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.DTOs
{
    public class CartItemResponseDTO
    {
        // ID of the cart item.
        public int CartItemId { get; set; }
        // ID of the product.
        public int ProductId { get; set; }
        // Name of the product.
        public string? ProductName { get; set; }
        // Description of the product.
        public string? Description { get; set; }
        // Unit price of the product.
        public decimal UnitPrice { get; set; }
        // Total price for this cart item (UnitPrice * Quantity).
        public decimal TotalPrice { get; set; }
        // Quantity of the product in the cart.
        public int Quantity { get; set; }
        // Price after applying discounts.
        public decimal DiscountedPrice { get; set; }
    }
}