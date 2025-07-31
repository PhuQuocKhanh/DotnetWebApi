using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBinding.Models
{
    public class ProductFromRoute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Categogy { get; set; }
        public int Price { get; set; }
    }

    public class ProductRoute
    { 
        public string Name { get; set; }
        public string Category { get; set; }
    }
}