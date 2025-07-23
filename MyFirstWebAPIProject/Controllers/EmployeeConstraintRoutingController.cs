using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeConstraintRoutingController : ControllerBase
    {
        [Route("{EmployeeId:int}")]  // Ràng buộc kiểu integer
        [HttpGet]
        public string GetEmployeeIntDetails(int EmployeeId)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeId: {EmployeeId}";
        }

        [Route("{EmployeeName}")]
        [HttpGet]
        public string GetEmployeeDetails(string EmployeeName)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeName: {EmployeeName}";
        }

        [Route("{EmployeeIdMin:int:min(1000)}")]
        [HttpGet]
        public string GetEmployeeMinDetails(int EmployeeId)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeId : {EmployeeId}";
        }

        [Route("{EmployeeNameAlpha:alpha}")]
        [HttpGet]
        public string GetEmployeeAlphaDetails(string EmployeeName)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeName : {EmployeeName}";
        }

        [Route("{EmployeeIdMax:int:max(1000)}")]
        [HttpGet]
        public string GetEmployeeMaxDetails(int EmployeeId)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeId : {EmployeeId}";
        }

        [Route("{EmployeeIdRange:int:min(100):max(1000)}")]
        [HttpGet]
        public string GetEmployeeMinMaxDetails(int EmployeeId)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeId : {EmployeeId}";
        }

        [Route("{EmployeeNameAlphaMin:alpha:minlength(5)}")]
        [HttpGet]
        public string GetEmployeeAlphaMinLengthDetails(string EmployeeName)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeName : {EmployeeName}";
        }

        [Route("{EmployeeNameAlphaMax:alpha:maxlength(10)}")]
        [HttpGet]
        public string GetEmployeeAlphaMaxLengthDetails(string EmployeeName)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeName : {EmployeeName}";
        }

        [Route("{EmployeeNameAlphaLength:alpha:length(5)}")]
        [HttpGet]
        public string GetEmployeeSameAlphaLengthDetails(string EmployeeName)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeName : {EmployeeName}";
        }

        [Route("{EmployeeNameRegex:regex(a(b|c))}")]
        [HttpGet]
        public string GetEmployeeRegexDetails(string EmployeeName)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeName : {EmployeeName}";
        }
    }
}