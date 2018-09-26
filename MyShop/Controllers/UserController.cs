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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string maleHeadphotoUrl = "/images/userHeadphoto/male_headphoto.png";
        private const string femaleHeadphotoUrl = "/images/userHeadphoto/female_headphoto.png";

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> GetAsync()
        {
            var iuser = await _userManager.GetUserAsync(User);
            var name = User.Identity.Name;
            var gender = User.FindFirstValue(ClaimTypes.Gender);
            var age = User.FindFirstValue(ClaimTypesExtensions.Age);
            var locality = User.FindFirstValue(ClaimTypes.Locality);
            var phone = iuser.PhoneNumber;
            var headphotoUrl = User.FindFirstValue(ClaimTypesExtensions.HeadphotoUrl);

            return Ok(new
            {
                Name = name,
                Gender = gender,
                Age = age,
                Locality = locality,
                Phone = phone,
                HeadphotoUrl = string.IsNullOrEmpty(headphotoUrl) ? maleHeadphotoUrl : headphotoUrl
            });
        }

        [HttpPost("info")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateInfoAsync([FromBody] UpdateInfoJson infoJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.First().Value.Errors.First().ErrorMessage);
            }

            var iuser = await _userManager.GetUserAsync(User);
            await UpdateClaimAsync(iuser, ClaimTypes.Gender, new Claim(ClaimTypes.Gender, infoJson.Gender));
            await UpdateClaimAsync(iuser, ClaimTypesExtensions.Age, new Claim(ClaimTypesExtensions.Age, infoJson.Age));
            await UpdateClaimAsync(iuser, ClaimTypes.Locality, new Claim(ClaimTypes.Locality, infoJson.Locality));
            await UpdateClaimAsync(iuser, ClaimTypesExtensions.HeadphotoUrl, new Claim(ClaimTypesExtensions.HeadphotoUrl, infoJson.Gender.Equals("male") ? maleHeadphotoUrl : femaleHeadphotoUrl));
            iuser.PhoneNumber = infoJson.Phone;
            await _signInManager.RefreshSignInAsync(iuser);
            await _userManager.UpdateAsync(iuser);
            return Ok();
        }

        private async Task<IdentityResult> UpdateClaimAsync(ApplicationUser user, string claimType, Claim newClaim)
        {
            var oldClaims = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == claimType);
            if (oldClaims == null)
            {
                return await _userManager.AddClaimAsync(user, newClaim);
            }
            else
            {
                return await _userManager.ReplaceClaimAsync(user, oldClaims, newClaim);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginJson loginInfo)
        {
            //认证登录用户角色是否客人
            var user = await _userManager.FindByNameAsync(loginInfo.Name);
            if(!(await _userManager.GetClaimsAsync(user)).Any(c => c.Type == ClaimTypes.Role && c.Value == "customer"))
            {
                return BadRequest("用户名或密码错误！");
            }

            var result = await _signInManager.PasswordSignInAsync(loginInfo.Name, loginInfo.Password, false, false);
            
            if(!result.Succeeded)
            {
                return BadRequest("用户名或密码错误！");
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterJson registerInfo)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.First().Value.Errors.First().ErrorMessage);
            }

            var user = new ApplicationUser() { UserName = registerInfo.Name, Email = registerInfo.Email };
            var result = await _userManager.CreateAsync(user, registerInfo.Password);
            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Gender, "male"));
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

        public class UpdateInfoJson
        {
            [Required]
            public string Gender { get; set; }
            [Range(10, 99, ErrorMessage = "请输入正确的年龄")]
            public string Age { get; set; }
            [Required(ErrorMessage ="地区是必须的")]
            public string Locality { get; set; }
            [Required(ErrorMessage ="手机号码是必须的")]
            [RegularExpression("^[1][3,4,5,7,8][0-9]{9}$", ErrorMessage = "请填写正确的手机号码")]
            public string Phone { get; set; }
        }
    }
}
