using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCacheDemo.Model.InMemoryCache
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        // Navigation property to states belonging to this country
        public List<State> States { get; set; }
    }
}