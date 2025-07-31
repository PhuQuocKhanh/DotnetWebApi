using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ModelBinding.Models
{
    public class Book
    {
        [FromRoute(Name = "id")] // Binds to the route parameter {id}
        public int Id { get; set; }
        [FromQuery] // Binds to the query string parameter 'category'
        public string Category { get; set; }
        [FromHeader(Name = "X-User-Id")] // Binds to the header 'X-User-Id'
        public string UserId { get; set; }
        [FromBody] // Binds to the request body
        public BookDetails Details { get; set; }
    }
    public class BookDetails
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Content { get; set; }
    }
}