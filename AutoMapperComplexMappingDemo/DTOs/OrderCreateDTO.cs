using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapperComplexMappingDemo.DTOs
{
    public class OrderCreateDTO
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public List<OrderItemCreateDTO> Items { get; set; }
        // The amount can be calculated on the server side based on the product prices and quantities.
        // However, if you prefer to include it, you can add it here.
    }
}