using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomapperNullSubstitute.DTOs
{
    public class EmployeeDTOComplexType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public AddressDTOComplexType Address { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}