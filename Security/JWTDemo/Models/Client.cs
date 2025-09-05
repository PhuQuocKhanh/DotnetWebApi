using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo.Models
{
    [Index(nameof(ClientId), Name = "IX_Unique_ClientId", IsUnique = true)]
    public class Client
    {
        [Key]
        public int Id { get; set; }

        // Định danh duy nhất cho ứng dụng client.
        [Required(ErrorMessage = "Định danh Client là bắt buộc.")]
        [MaxLength(50)]
        public string ClientId { get; set; } = null!;

        // Tên của ứng dụng client.
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Mã bí mật Client là bắt buộc.")]
        [MaxLength(100)]
        public string ClientSecret { get; set; } = null!;

        // URL cho ứng dụng client.
        [Required]
        [MaxLength(200)]
        public string ClientURL { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; } = true;
    }
}