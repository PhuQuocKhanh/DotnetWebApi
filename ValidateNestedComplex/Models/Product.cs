using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ValidateNestedComplex.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } // Bắt buộc, độ dài tối đa 100
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; } // Phải lớn hơn 0
        public int Quantity { get; set; } // Số lượng tồn kho, phải >= 0
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}