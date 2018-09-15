using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using MyShop.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<object> Get()
        {
            return new { Name = HttpContext.User.Identity.Name };
        }
        
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginJson loginInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(loginInfo.Name, loginInfo.Password, false, false);
            if(result.Succeeded)
            {
                return Ok();
            }

            return BadRequest("用户名或密码错误！");
        }

        [HttpPut]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterJson registerInfo)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.First().Value.Errors.First().ErrorMessage);
            }

            var user = new IdentityUser() { UserName = registerInfo.Name, Email = registerInfo.Email };
            var result = await _userManager.CreateAsync(user, registerInfo.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors.First().Description);
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        public class UserLoginJson
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public bool IsPersistent { get; set; }
        }

        public class UserRegisterJson
        {
            [Required(ErrorMessage = "用户名是必须的")]
            public string Name { get; set; }
            [Required(ErrorMessage = "密码是必须的")]
            public string Password { get; set; }
            [Compare("Password", ErrorMessage = "两次密码输入不一样！")]
            public string ConfirmPassword { get; set; }
            [Required(ErrorMessage = "邮箱是必须的")]
            [EmailAddress(ErrorMessage = "邮箱格式不正确")]
            public string Email { get; set; }
        }
    }
}
