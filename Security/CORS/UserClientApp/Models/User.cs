using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserClientApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Position { get; set; } = null!;
    }
}