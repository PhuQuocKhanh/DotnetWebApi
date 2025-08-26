using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationFilters.Models
{
    public class User
    {
        // ID duy nhất của người dùng
        public int Id { get; set; }
        // Tên của người dùng
        public string Name { get; set; } = null!;
        // Tên đăng nhập của người dùng
        public string Email { get; set; } = string.Empty;
        // Mật khẩu chỉ để demo (trong ứng dụng thực tế, hãy lưu mật khẩu đã được hash)
        public string Password { get; set; } = string.Empty;
        // Các vai trò được gán cho người dùng, phân tách bằng dấu phẩy
        public string Roles { get; set; } = string.Empty;
    }
}