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
    public class UserUsingAttributeController : ControllerBase
    {
        private readonly IMapper _mapper;

        public UserUsingAttributeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{Id}")]
        public IActionResult GetUser(int Id)
        {
            var user = new UserUsingAttribute
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