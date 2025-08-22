using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPICustomValidator.Models
{
    public class User
    {
        public int UserId { get; set; }          // Khóa chính (Primary Key)
        public string FirstName { get; set; }      // Tên người dùng
        public string LastName { get; set; }       // Họ người dùng
        public string Email { get; set; }          // Địa chỉ email người dùng, phải là duy nhất (Unique)
        public string Password { get; set; }       // Mật khẩu người dùng (nên được băm trong môi trường production)
        public DateTime DateOfBirth { get; set; }    // Ngày sinh người dùng
        public string PhoneNumber { get; set; }    // Số điện thoại liên lạc
        public string Address { get; set; }        // Tùy chọn: Địa chỉ người dùng
        public int? GenderId { get; set; }         // Khóa ngoại tới Gender
        public int? CountryId { get; set; }        // Khóa ngoại tới Country
        public int? CityId { get; set; }           // Khóa ngoại tới City (phải thuộc về Country đã chọn)
        // Các thuộc tính điều hướng (Navigation Properties)
        public Gender Gender { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
    }
}