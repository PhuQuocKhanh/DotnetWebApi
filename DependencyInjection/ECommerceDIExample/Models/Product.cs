using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.Models
{
    public class Product
    {
        public int Id { get; set; }
        // Common product details
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string? Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public decimal Price { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
        [Range(0, 1000, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        // Soft delete indicator
        public bool IsAvailable { get; set; } = true;
    }
}