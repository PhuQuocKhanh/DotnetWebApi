using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidateNestedComplex.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } // Bắt buộc, độ dài tối đa 50
        public string Email { get; set; } // Bắt buộc, định dạng email hợp lệ
        public string? Phone { get; set; } // Tùy chọn, số điện thoại hợp lệ
        public ICollection<Order> Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}