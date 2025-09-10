using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JWTAuthServer.Models
{
    [Index(nameof(Token), Name = "IX_Token_Unique", IsUnique = true)]
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        // Chuỗi refresh token (nên là một chuỗi ngẫu nhiên an toàn)
        [Required]
        public string Token { get; set; }

        // Người dùng được liên kết với refresh token
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Client được liên kết với refresh token
        [Required]
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }

        // Ngày hết hạn của token
        [Required]
        public DateTime ExpiresAt { get; set; }

        // Cho biết token đã bị thu hồi hay chưa
        [Required]
        public bool IsRevoked { get; set; } = false;

        // Ngày token được tạo
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Ngày token bị thu hồi
        public DateTime? RevokedAt { get; set; }
    }
}