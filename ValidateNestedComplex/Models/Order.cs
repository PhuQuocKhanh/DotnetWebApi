using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ValidateNestedComplex.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } // Khóa ngoại
        public Customer Customer { get; set; } // Thuộc tính điều hướng (Navigation property)
        public int ShippingAddressId { get; set; } // Khóa ngoại
        public Address ShippingAddress { get; set; } // Thuộc tính điều hướng
        public DateTime OrderDate { get; set; } // Bắt buộc
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OrderAmount { get; set; } // Được tính toán dựa trên OrderItems
        public ICollection<OrderItem> OrderItems { get; set; } // Phải chứa ít nhất một mặt hàng
    }
}