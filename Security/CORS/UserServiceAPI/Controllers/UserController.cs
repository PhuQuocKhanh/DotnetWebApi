using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserServiceAPI.Models;

namespace UserServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;
        public UserController(UserDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
           var users = await _context.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(usr => usr.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        [EnableCors("AllowSpecificOrigin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        [DisableCors]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}