using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPICustomValidator.Data;
using FluentAPICustomValidator.DTOs;
using FluentAPICustomValidator.Models;
using FluentAPICustomValidator.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FluentAPICustomValidator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _dbContext;
        public UsersController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Đăng ký người dùng mới sau khi xác thực các chi tiết được cung cấp.
        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserDTO createUserDTO)
        {
            // Khởi tạo validator với DbContext hiện tại.
            var validator = new UserDTOValidator(_dbContext);
            var validationResult = await validator.ValidateAsync(createUserDTO);

            // Nếu xác thực thất bại, trả về phản hồi lỗi đầy đủ
            //if (!validationResult.IsValid)
            //{
            //    return BadRequest(validationResult.Errors);
            //}
            
            // Nếu xác thực thất bại, ánh xạ lỗi sang một phản hồi đơn giản và trả về 400 Bad Request.
            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });
                return BadRequest(new { Errors = errorResponse });
            }
            // Ánh xạ DTO đã được xác thực sang thực thể User.
            var user = new User
            {
                FirstName = createUserDTO.FirstName,
                LastName = createUserDTO.LastName,
                Email = createUserDTO.Email,
                Password = createUserDTO.Password, 
                DateOfBirth = createUserDTO.DateOfBirth,
                PhoneNumber = createUserDTO.PhoneNumber,
                Address = createUserDTO.Address,
                GenderId = createUserDTO.GenderId,
                CountryId = createUserDTO.CountryId,
                CityId = createUserDTO.CityId,
            };
            // Thêm người dùng mới vào cơ sở dữ liệu.
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            // Trả về người dùng đã tạo như một phản hồi.
            return Ok(user);
        }
    }
}