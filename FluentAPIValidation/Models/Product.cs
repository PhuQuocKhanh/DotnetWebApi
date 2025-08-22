using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIValidation.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        // Mã SKU (Stock Keeping Unit) theo pattern cụ thể, ví dụ: 8 ký tự chữ in hoa hoặc số
        public string SKU { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }

        // Phần trăm giảm giá (0-100)
        [Column(TypeName ="decimal(18,2)")]
        public decimal Discount { get; set; }

        // Ngày sản xuất (không được lớn hơn ngày hiện tại)
        public DateTime ManufacturingDate { get; set; }

        // Ngày hết hạn (phải sau ngày sản xuất)
        public DateTime ExpiryDate { get; set; }

        // Quan hệ many-to-many với Tag
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}