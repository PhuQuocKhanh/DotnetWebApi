using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBinding.Models;

namespace ModelBinding.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserFromQueryController : ControllerBase
    {
        private static List<UserModelFromQuery> Users = new List<UserModelFromQuery>
        {
            new UserModelFromQuery { Id = 1, Name = "Rakesh", Department = "IT", Gender = "Male", Salary = 1000 },
            new UserModelFromQuery { Id = 2, Name = "Priyanka", Department = "IT", Gender = "Female", Salary = 2000  },
            new UserModelFromQuery { Id = 3, Name = "Suresh", Department = "HR", Gender = "Male", Salary = 3000 },
            new UserModelFromQuery { Id = 4, Name = "Hina", Department = "HR", Gender = "Female", Salary = 4000 },
            new UserModelFromQuery { Id = 5, Name = "Pranaya", Department = "HR", Gender = "Male", Salary = 35000 },
            new UserModelFromQuery { Id = 6, Name = "Pooja", Department = "IT", Gender = "Female", Salary = 2500 },
        };

        [HttpGet]
        public IActionResult GetProducts([FromQuery] string Department)
        {
            // Implementation to retrieve employees based on the Department
            var FilteredUsers = Users.Where(emp => emp.Department.Equals(Department, StringComparison.OrdinalIgnoreCase)).ToList();
            if (FilteredUsers.Count > 0)
            {
                return Ok(FilteredUsers);
            }
            return NotFound($"No Users Found with Department: {Department}");
        }

        [HttpGet]
        public IActionResult GetProductsCustomize([FromQuery(Name = "Dept")] string Department)
        {
            // Implementation to retrieve employees based on the Department
            var FilteredUsers = Users.Where(emp => emp.Department.Equals(Department, StringComparison.OrdinalIgnoreCase)).ToList();
            if (FilteredUsers.Count > 0)
            {
                return Ok(FilteredUsers);
            }
            return NotFound($"No Users Found with Department: {Department}");
        }
        
        [HttpGet]
        public IActionResult GetUsers([FromQuery] UserSearch userSearch)
        {
            var FilteredUsers = new List<UserModelFromQuery>();
            if (userSearch != null)
            {
                FilteredUsers = [.. Users.Where(
                       emp => emp.Department.Equals(userSearch.Department, StringComparison.OrdinalIgnoreCase) ||
                       emp.Gender.Equals(userSearch.Gender, StringComparison.OrdinalIgnoreCase)
                       )];
                if (FilteredUsers.Count > 0)
                {
                    return Ok(FilteredUsers);
                }
                return NotFound($"No Users Found with Department: {userSearch?.Department} and Gender: {userSearch?.Gender}");
            }
            return BadRequest("Invalid Search Criteria");
        }
    }
}