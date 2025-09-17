using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceDIExample.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [Required]
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity should be at least 1.")]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.0, double.MaxValue, ErrorMessage = "UnitPrice cannot be negative.")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.0, double.MaxValue, ErrorMessage = "TotalPrice cannot be negative.")]
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}