using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthServer.DTOs
{
    public class LogoutRequestDTO
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string ClientId { get; set; }

        public bool IsLogoutFromAllDevices { get; set; }
    }
}