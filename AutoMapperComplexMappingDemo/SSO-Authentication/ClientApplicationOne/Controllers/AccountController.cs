using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClientApplicationOne.Models;
using ClientApplicationOne.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClientApplicationOne.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserAuthenticationService _userAuthenticationService;

        public AccountController(UserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }

        [HttpGet("register")]
        public IActionResult Register() => View();

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _userAuthenticationService.RegisterUserAsync(model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Login");

            ModelState.AddModelError("", "Registration Failed.");
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login() => View();

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _userAuthenticationService.LoginUserAsync(model);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponseModel>(responseContent);
                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    HttpContext.Session.SetString("JWT", loginResponse.Token);
                    HttpContext.Session.SetString("Username", model.Username);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Token not found in the response.");
            }
            ModelState.AddModelError("", "Login failed.");
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWT");
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("validate-sso")]
        public async Task<IActionResult> ValidateSSO([FromQuery] string ssoToken)
        {
            if (string.IsNullOrEmpty(ssoToken)) return RedirectToAction("Login");

            var model = new ValidateSSOTokenModel { SSOToken = ssoToken };
            var response = await _userAuthenticationService.ValidateSSOTokenAsync(model);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var ssoResponse = JsonSerializer.Deserialize<ValidateSSOResponseModel>(responseContent);

                if (ssoResponse != null)
                {
                    if (!string.IsNullOrEmpty(ssoResponse.Token))
                        HttpContext.Session.SetString("JWT", ssoResponse.Token);

                    if (!string.IsNullOrEmpty(ssoResponse.UserDetails?.Username))
                        HttpContext.Session.SetString("Username", ssoResponse.UserDetails.Username);

                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Login");
        }
    }

}