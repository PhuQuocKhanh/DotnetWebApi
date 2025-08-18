using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCacheDemo.Model.InMemoryCache
{
    public class State
    {
        public int StateId { get; set; }
        public string Name { get; set; }
        // Foreign key reference to the Country
        public int CountryId { get; set; }
        // Navigation property to the parent Country
        public Country Country { get; set; }
        // Navigation property to cities belonging to this state
        public List<City> Cities { get; set; }
    }
}