using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModelBinding.Models
{
    public class EmployeeExcludePropertiesDTO
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }
    }
    public class EmployeeExcludeProperties
    {
        //[JsonIgnore]
        public int Id { get; set; }  // Sensitive property
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        //[JsonIgnore]
        public int Salary { get; set; }  // Sensitive property
        public string Department { get; set; }
    }
}