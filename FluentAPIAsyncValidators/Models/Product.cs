using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIAsyncValidators.Models
{
    public class Product
    {
        public int ProductId { get; set; } // Primary Key
        public string Name { get; set; } // Unique product name
        public string SKU { get; set; }  // Unique Stock Keeping Unit
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; } // Product price
        public int CategoryId { get; set; } // Foreign key to Category
        public int Stock { get; set; } // Inventory quantity
        public string? Description { get; set; } // Optional product description
        [Column(TypeName = "decimal(10,2)")]
        public decimal Discount { get; set; } // Discount percentage
        // Navigation property: The related category for the product.
        public Category Category { get; set; }
    }
}