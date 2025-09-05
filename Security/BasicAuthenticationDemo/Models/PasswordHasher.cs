using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuthenticationDemo.Models
{
    public static class PasswordHasher
    {
        // Băm một mật khẩu dạng văn bản thô thành một chuỗi băm SHA-256 được mã hóa Base64.
        public static string HashPassword(string password)
        {
            // Tạo một instance mới của thuật toán băm SHA256.
            using var sha256 = SHA256.Create();
            // Chuyển đổi chuỗi mật khẩu đầu vào thành một mảng byte sử dụng mã hóa UTF-8.
            var bytes = Encoding.UTF8.GetBytes(password);
            // Tính toán giá trị băm SHA-256 của các byte mật khẩu.
            var hash = sha256.ComputeHash(bytes);
            // Chuyển đổi mảng byte đã băm thành một chuỗi mã hóa Base64 để dễ dàng lưu trữ hoặc so sánh.
            return Convert.ToBase64String(hash);
        }

        // Xác minh xem mật khẩu thô được cung cấp có khớp với mật khẩu đã băm được lưu trữ hay không.
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            // Băm mật khẩu thô được cung cấp và so sánh nó với chuỗi băm đã lưu.
            // Trả về true nếu cả hai chuỗi băm khớp nhau, cho biết mật khẩu là chính xác.
            return hashedPassword == HashPassword(providedPassword);
        }
    }
}