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
    public class DataAnnotationController : ControllerBase
    {
        // Danh sách người dùng trong bộ nhớ tạm (dùng cho demo)
        private static readonly List<DataAnnotation> Users = new List<DataAnnotation>();

        // POST: api/Users
        [HttpPost]
        public IActionResult CreateDataAnnotation([FromBody] DataAnnotation user)
        {
            // Kiểm tra ModelState, nếu không hợp lệ sẽ trả về lỗi
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Gán ID và thêm user vào danh sách
            user.Id = Users.Count + 1;
            Users.Add(user);

            return CreatedAtAction(nameof(GetDataAnnotationById), new { id = user.Id }, user);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public IActionResult GetDataAnnotationById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // GET: api/Users
        [HttpGet]
        public IActionResult GetAllDataAnnotation()
        {
            return Ok(Users);
        }
    }
}