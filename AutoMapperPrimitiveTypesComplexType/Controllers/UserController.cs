using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperPrimitiveTypesComplexType.Data;
using AutoMapperPrimitiveTypesComplexType.DTOs;
using AutoMapperPrimitiveTypesComplexType.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMapperPrimitiveTypesComplexType.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserDbContext _context;

        public UserController(IMapper mapper, UserDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/user/GetUsers
        // Trả về toàn bộ danh sách người dùng.
        // Sử dụng AutoMapper để chuyển từ entity User (có Address dạng complex) sang UserDTO (các thuộc tính dạng primitive).
        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Include(u => u.Address)
                .ToListAsync();

            if (users == null || users.Count == 0)
                return NotFound("No users found.");

            var userDTOs = _mapper.Map<List<UserDTO>>(users);
            return Ok(userDTOs);
        }

        // GET: api/user/GetUserById/{id}
        // Trả về thông tin người dùng theo ID
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound($"User with ID {id} not found.");

            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        // POST: api/user/CreateUser
        // Tạo mới người dùng từ DTO có các thuộc tính nguyên thủy, ánh xạ sang entity User có Address dạng complex
        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO userCreateDTO)
        {
            var user = _mapper.Map<User>(userCreateDTO);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }
    }
}