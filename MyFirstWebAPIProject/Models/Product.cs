using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }
        
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }
        
        [Required]
        public string Category { get; set; }
    }
}