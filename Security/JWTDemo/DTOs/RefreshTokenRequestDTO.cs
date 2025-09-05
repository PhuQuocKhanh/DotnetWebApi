using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWTDemo.DTOs
{
    public class RefreshTokenRequestDTO
    {
        [Required(ErrorMessage = "Refresh Token là bắt buộc.")]
        public string RefreshToken { get; set; } = null!;

        [Required(ErrorMessage = "Client Id là bắt buộc.")]
        public string ClientId { get; set; } = null!;
    }
}