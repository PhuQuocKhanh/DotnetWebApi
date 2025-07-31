using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoMapperIgnoreProperty.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Thông tin nhạy cảm
        public string SecurityToken { get; set; } // Thông tin nhạy cảm
    }
}