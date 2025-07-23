using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPIProject.Models;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeReturnTypeController : ControllerBase
    {
        [HttpGet("Primitive-Types")]
        public string ReturningPrimitiveTypes()
        {
            return "Return Returning Primitive Types from GetName";
        }

        [HttpGet("Complex-Type")]
        public EmployeeReturnType GetEmployeeDetails()
        {
            return new EmployeeReturnType()
            {
                Id = 1001,
                Name = "Anurag",
                Age = 28,
                City = "Mumbai",
                Gender = "Male",
                Department = "IT"
            };
        }
    }
}