using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordHashingDemo.Models
{
    // Cung cấp các phương thức để tạo và xác minh hash mật khẩu bằng HMACSHA512.
    public static class PasswordHasher
    {
        // Tạo một phiên bản hash của mật khẩu được cung cấp cùng với một salt duy nhất.
        // password: Mật khẩu plain text cần được hash.
        // passwordHash: Đầu ra là chuỗi hash của mật khẩu dưới dạng mảng byte.
        // passwordSalt: Đầu ra là salt duy nhất được sử dụng trong quá trình hash dưới dạng mảng byte.
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Khởi tạo HMACSHA512 để tạo một hash mật mã và một khóa duy nhất (salt).
            using (var hmac = new HMACSHA512())
            {
                // Thuộc tính Key của HMACSHA512 cung cấp một salt được tạo ngẫu nhiên.
                passwordSalt = hmac.Key; // Gán salt được tạo cho tham số đầu ra.

                // Chuyển đổi mật khẩu plain text thành mảng byte bằng mã hóa UTF-8.
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Tính toán hash của mảng byte mật khẩu bằng HMACSHA512.
                passwordHash = hmac.ComputeHash(passwordBytes); // Gán hash đã tính toán cho tham số đầu ra.
            }
        }

        // Xác minh xem mật khẩu được cung cấp có khớp với hash đã lưu bằng cách sử dụng salt đã lưu hay không.
        // password: Mật khẩu plain text cần xác minh.
        // storedHash: Hash mật khẩu đã lưu để so sánh.
        // storedSalt: Salt đã lưu được sử dụng trong quá trình hash ban đầu.
        // Trả về: True nếu mật khẩu hợp lệ và khớp với hash đã lưu; ngược lại là false.
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            // Khởi tạo HMACSHA512 với salt đã lưu làm khóa để đảm bảo cùng tham số hash.
            using (var hmac = new HMACSHA512(storedSalt))
            {
                // Chuyển đổi mật khẩu plain text thành mảng byte bằng mã hóa UTF-8.
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Tính toán hash của mảng byte mật khẩu bằng HMACSHA512 được khởi tạo với salt đã lưu.
                byte[] computedHash = hmac.ComputeHash(passwordBytes);

                // So sánh từng byte của hash vừa tính toán với hash đã lưu.
                // SequenceEqual đảm bảo cả hai mảng byte đều giống hệt nhau về thứ tự và giá trị.
                bool hashesMatch = computedHash.SequenceEqual(storedHash);

                // Trả về kết quả so sánh.
                return hashesMatch;
            }
        }
    }
}