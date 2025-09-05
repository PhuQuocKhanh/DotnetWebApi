using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuthenticationDemo.Models
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Product Name must be between 2 and 150 characters.")]
        public string Name { get; set; } = null!;
        
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = null!;
        
        [Range(0.01, 9999999999.99, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
    }

    public class UpdateProductDTO
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int Id { get; set; }

        [StringLength(150, MinimumLength = 2, ErrorMessage = "Product Name must be between 2 and 150 characters.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
        
        [Range(0.01, 9999999999.99, ErrorMessage = "Price must be greater than 0.")]
        public decimal? Price { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int? Stock { get; set; }
    }
}