using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBinding.Models;

namespace ModelBinding.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        [HttpPost("{id}")]
        public IActionResult CreateBook(Book request)
        {
            // Logic to store the data in the database would go here.
            // For demonstration, we'll return the received data as a response.
            var response = new
            {
                BookId = request.Id,
                Category = request.Category,
                UserId = request.UserId,
                Title = request.Details.Title,
                Author = request.Details.Author,
                Year = request.Details.Year
            };
            return Ok(response);
        }
    }
}