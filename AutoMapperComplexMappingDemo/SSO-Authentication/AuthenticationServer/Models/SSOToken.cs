using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.Models
{
    public class SSOToken
    {
        public int Id { get; set; }                   // ID duy nhất cho bản ghi token
        public string UserId { get; set; } = null!;   // Liên kết token với một người dùng cụ thể
        public string Token { get; set; } = null!;    // Chuỗi token SSO (thường là một GUID)
        public DateTime ExpiryDate { get; set; }      // Thời điểm token hết hạn
        public bool IsUsed { get; set; }              // Cho biết token đã được sử dụng hay chưa
        public bool IsExpired => DateTime.UtcNow > ExpiryDate; // Thuộc tính tính toán để kiểm tra xem token đã hết hạn chưa.
    }
}