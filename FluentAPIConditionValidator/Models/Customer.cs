using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPIConditionValidator.Models
{
 // Đại diện cho một khách hàng trong hệ thống thương mại điện tử.
    public class Customer
    {
        public int CustomerId { get; set; }
        // Tên đầy đủ của khách hàng
        public string Name { get; set; }
        // Thuộc tính điều hướng cho các đơn hàng liên quan
        public ICollection<Order> Orders { get; set; }
    }
}