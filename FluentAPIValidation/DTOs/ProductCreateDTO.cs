using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIValidation.DTOs
{
    public class ProductCreateDTO
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public decimal Discount { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        // Danh sách tên tag (sẽ được chuẩn hóa và lưu vào bảng Tags)
        public List<string>? Tags { get; set; }
    }
}