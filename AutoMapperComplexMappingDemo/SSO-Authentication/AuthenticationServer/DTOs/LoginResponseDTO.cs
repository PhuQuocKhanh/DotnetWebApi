using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationServer.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
    }
}