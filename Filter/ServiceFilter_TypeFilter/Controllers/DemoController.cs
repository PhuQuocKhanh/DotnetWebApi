using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceFilter_TypeFilter.Filters;

namespace ServiceFilter_TypeFilter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        // Sử dụng ServiceFilter: filter được đăng ký trong DI và sử dụng các phụ thuộc được tiêm vào
        [HttpGet("with-servicefilter")]
        [ServiceFilter(typeof(ExecutionTimeLoggingFilter))]
        public IActionResult WithServiceFilter()
        {
            return Ok("ServiceFilter executed!");
        }

        // Sử dụng TypeFilter: filter nhận một đối số lúc chạy cùng với các phụ thuộc được tiêm vào
        [HttpGet("with-typefilter")]
        [TypeFilter(typeof(CustomValidationFilter), Arguments = new object[] { "X-Api-Key" })]
        public IActionResult WithTypeFilter()
        {
            return Ok("TypeFilter executed!");
        }
    }
}