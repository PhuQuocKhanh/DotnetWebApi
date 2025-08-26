using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidateNestedComplex.DTOs
{
    public class OrderDTO
    {
        public int CustomerId { get; set; } // Bắt buộc
        // Nếu được cung cấp (> 0), phải tham chiếu đến một địa chỉ đã tồn tại của khách hàng.
        // Nếu null hoặc 0, thì thuộc tính NewAddress sẽ được sử dụng.
        public int? ShippingAddressId { get; set; }
        // Chi tiết NewAddress là bắt buộc nếu ShippingAddressId là null hoặc 0.
        public AddressDTO? NewAddress { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } // Phải chứa ít nhất một mặt hàng
    }
}