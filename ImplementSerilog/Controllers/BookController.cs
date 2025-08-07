using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImplementSerilog.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImplementSerilog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        // Giả lập dữ liệu với danh sách sách tĩnh
        private static List<Book> Books =
        [
            new (){ Id = 1001, Title = "ASP.NET Core", Author = "Pranaya", YearPublished = 2019 },
            new (){ Id = 1002, Title = "SQL Server", Author = "Pranaya", YearPublished = 2022 }
        ];

        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            Books.Add(book);
            _logger.LogInformation("Added a new book {@Book}", book); // Ghi log có cấu trúc
            return Ok();
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            _logger.LogInformation("Retrieved all books. Books: {@Books}", Books); // Ghi log danh sách có cấu trúc
            return Ok(Books);
        }
    }
}