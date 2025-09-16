using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVersioning.DTOs
{
    // Request DTO cho product (v1)
    public class ProductCreateRequestV1
    {
        public string Name { get; set; } = null!;
    }

    // Response DTO cho product (v1)
    public class ProductResponseV1
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    // Request DTO cho product (v2)
    public class ProductCreateRequestV2
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
    }

    // Response DTO cho product (v2)
    public class ProductResponseV2
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
    }
}