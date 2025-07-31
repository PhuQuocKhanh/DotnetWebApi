using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapperDemo.DTOs
{
    public class ProductCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        // Sensitive/internal fields
        public decimal SupplierCost { get; set; }
        public string SupplierInfo { get; set; }
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
    }
}