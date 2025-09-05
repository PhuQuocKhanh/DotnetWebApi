using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo.Models
{
    [Index(nameof(Token), Name = "IX_Token_Unique", IsUnique = true)]
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        // Chuỗi refresh token (nên là một chuỗi ngẫu nhiên an toàn).
        [Required]
        public string Token { get; set; } = null!;

        // Giúp vô hiệu hóa refresh token khi access token liên quan bị thu hồi hoặc nghi ngờ bị xâm phạm.
        [Required]
        public string JwtId { get; set; } = null!;

        // Ngày hết hạn của token.
        [Required]
        public DateTime Expires { get; set; }

        // Cho biết token đã bị thu hồi hay chưa.
        public bool IsRevoked { get; set; } = false;

        // Ngày token bị thu hồi.
        public DateTime? RevokedAt { get; set; }

        // Người dùng liên kết với refresh token.
        // Khóa ngoại cho người dùng liên quan.
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Ngày token được tạo.
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Client liên kết với refresh token.
        [Required]
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;

        public string? CreatedByIp { get; set; }
    }
}