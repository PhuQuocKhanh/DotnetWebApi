using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperIgnoreProperty.DTOs;
using AutoMapperIgnoreProperty.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoMapperIgnoreProperty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{Id}")]
        public IActionResult GetUser(int Id)
        {
            var user = new User
            {
                Id = Id,
                Username = "Test",
                Password = "123@ABC",
                SecurityToken = Guid.NewGuid().ToString(),
            };

            var userDTO = _mapper.Map<UserDTO>(user);

            return Ok(userDTO);
        }
    }
}