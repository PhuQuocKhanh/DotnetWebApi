using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMACServerApp.Models
{
    public class ClientSecret
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string ClientId { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string SecretKey { get; set; } = null!;
    }
}