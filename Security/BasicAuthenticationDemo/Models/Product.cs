using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuthenticationDemo.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Product Name must be between 2 and 150 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = null!;

        [Column(TypeName = "decimal(12,2)")]
        [Range(0.01, 9999999999.99, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
    }
}