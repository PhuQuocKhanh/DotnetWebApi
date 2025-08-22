using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPICustomValidator.DTOs
{
    public class UserDTO
    {
        // Chi tiết cá nhân
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Chi tiết liên lạc và bảo mật
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        // Thông tin nhân khẩu học
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderId { get; set; }
        // Chi tiết địa chỉ (tùy chọn)
        public string Address { get; set; }
        // Chi tiết vị trí
        public int CountryId { get; set; }
        public int CityId { get; set; } // CityId phải thuộc về Country đã chỉ định
    }
}