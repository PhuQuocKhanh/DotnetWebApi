using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPICustomValidator.Models
{
    public class Country
    {
        public int CountryId { get; set; } // Khóa chính (Primary Key)
        public string Name { get; set; }   // Tên quốc gia
        public ICollection<City> Cities { get; set; } // Danh sách các thành phố thuộc quốc gia này
    }
}