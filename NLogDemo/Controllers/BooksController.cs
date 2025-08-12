using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLogDemo.Models;

namespace NLogDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
         // Lưu trữ tạm trong bộ nhớ (chỉ dùng để demo)
        private static readonly List<Book> Books = new List<Book>()
        {
            new Book() { Id = 1001, Title = "ASP.NET Core", Author = "Pranaya", YearPublished = 2019 },
            new Book() { Id = 1002, Title = "SQL Server", Author = "Pranaya", YearPublished = 2022 }
        };

        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
        }

        // POST: Thêm sách mới
        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            Books.Add(book);

            // Sử dụng "@" để log object dưới dạng structured data (JSON)
            _logger.LogInformation("Added a new book {@Book}", book);

            return Ok();
        }

        // GET: Lấy danh sách tất cả sách
        [HttpGet]
        public IActionResult GetBooks()
        {
            // Log toàn bộ danh sách sách dưới dạng structured data
            _logger.LogInformation("Retrieved all books. Books: {@Books}", Books);

            return Ok(Books);
        }
    }
}