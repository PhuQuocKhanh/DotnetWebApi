using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidateNestedComplex.DTOs
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; } // Bắt buộc
        public int Quantity { get; set; } // Phải lớn hơn 0
    }
}