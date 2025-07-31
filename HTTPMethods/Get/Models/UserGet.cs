using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPMethods.Get.Models
{
    public class UserGet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<OrderGet> Orders { get; set; } = new List<OrderGet>();
    }
}