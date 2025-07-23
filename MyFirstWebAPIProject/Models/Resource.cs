using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstWebAPIProject.Models
{
    public class Resource
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime LastModified { get; set; }
    }
}