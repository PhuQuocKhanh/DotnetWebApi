using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIAsyncValidators.Models
{
    public class Category
    {
                public int CategoryId { get; set; } // Primary Key
        public string CategoryName { get; set; } // Name of the category
        public string? Description { get; set; } // Optional description of the category
        // Navigation property: A category can have multiple products.
        public List<Product> Products { get; set; }
    }
}