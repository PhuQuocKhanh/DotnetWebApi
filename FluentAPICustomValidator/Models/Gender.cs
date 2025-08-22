using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPICustomValidator.Models
{
    public class Gender
    {
        public int GenderId { get; set; } // Khóa chính (Primary Key)
        public string Name { get; set; }  // ví dụ: Male, Female, Unknown
    }
}