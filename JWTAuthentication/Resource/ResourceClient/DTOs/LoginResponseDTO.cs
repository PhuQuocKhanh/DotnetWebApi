using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceClient.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}