using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVersioning.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Thêm từ version 2
        public double? Price { get; set; }
    }
}