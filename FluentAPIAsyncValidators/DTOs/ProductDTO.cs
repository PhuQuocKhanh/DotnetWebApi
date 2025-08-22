using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIAsyncValidators.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; } // Product name
        public string SKU { get; set; }  // Unique identifier for the product
        public decimal Price { get; set; } // Product price
        public int CategoryId { get; set; } // Foreign key to Category
        public int Stock { get; set; } // Available inventory quantity
        public string? Description { get; set; } // Optional product description
        public decimal Discount { get; set; } // Discount percentage on the product
    }
}