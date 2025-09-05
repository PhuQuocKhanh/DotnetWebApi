using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AESServerAPP.Models
{
    public class ClientKeyIV
    {
        [Key]
        public int Id { get; set; }
        // Định danh duy nhất cho client.
        [Required]
        [MaxLength(50)]
        public string ClientId { get; set; }
        // Khóa AES được mã hóa Base64.
        [Required]
        public string Key { get; set; }
        // Vector Khởi tạo (IV) của AES được mã hóa Base64.
        [Required]
        public string IV { get; set; }
    }
}