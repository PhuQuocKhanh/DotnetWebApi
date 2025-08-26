using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ValidateNestedComplex.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Khóa ngoại
        public Order Order { get; set; } // Thuộc tính điều hướng
        public int ProductId { get; set; } // Khóa ngoại
        public Product Product { get; set; } // Thuộc tính điều hướng
        public int Quantity { get; set; } // Phải lớn hơn 0
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProductPrice { get; set; } // Giá tại thời điểm đặt hàng
    }
}