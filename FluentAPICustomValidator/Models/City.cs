using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAPICustomValidator.Models
{
    public class City
    {
        public int CityId { get; set; }    // Khóa chính (Primary Key)
        public string Name { get; set; }   // Tên thành phố
        public int CountryId { get; set; } // Khóa ngoại (Foreign Key) tới Country
        // Thuộc tính điều hướng (Navigation Property) để liên kết với thực thể Country
        public Country Country { get; set; }
    }
}