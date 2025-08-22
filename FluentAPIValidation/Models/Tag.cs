using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIValidation.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        // Quan hệ many-to-many với Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}