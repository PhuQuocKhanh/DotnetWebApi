using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBinding.Models
{
    public class OrderFormBody
    {
        public int Id { get; set; }
        public List<Product> Products { get; set; }
    }
    public class ProductFormBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}