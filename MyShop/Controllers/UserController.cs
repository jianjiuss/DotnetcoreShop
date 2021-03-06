﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.Util;
using Newtonsoft.Json;
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
        private readonly MyDbContext _dbcontext;
        private const string maleHeadphotoUrl = "/images/userHeadPhoto/male_headphoto.png";
        private const string femaleHeadphotoUrl = "/images/userHeadPhoto/female_headphoto.png";

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            MyDbContext dbcontext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbcontext = dbcontext;
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
            if(string.IsNullOrEmpty(loginInfo.Name))
            {
                return BadRequest("用户名不能为空");
            }

            if(string.IsNullOrEmpty(loginInfo.Password))
            {
                return BadRequest("密码不能为空");
            }

            //认证登录用户角色是否客人
            var user = await _userManager.FindByNameAsync(loginInfo.Name);
            if(user == null)
            {
                return BadRequest("用户名或密码错误！");
            }
            if(!(await _userManager.GetClaimsAsync(user)).Any(c => c.Type == ClaimTypes.Role && c.Value == "customer"))
            {
                return BadRequest("用户名或密码错误！");
            }

            var result = await _signInManager.PasswordSignInAsync(loginInfo.Name, loginInfo.Password, loginInfo.IsPersistent, false);
            
            if(!result.Succeeded)
            {
                return BadRequest("用户名或密码错误！");
            }

            //合拼购物车项目
            if(HttpContext.Request.Cookies.Any(c => c.Key.Equals("shopcart")))
            {
                var tempShopcart = JsonConvert.DeserializeObject<ShopCart>(HttpContext.Request.Cookies.First(c => c.Key.Equals("shopcart")).Value);
                var shopcart = _dbcontext.ShopCarts
                    .Include(s => s.ShopCartItems)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault(s => s.UserId == user.Id);
                if(shopcart == null)
                {
                    var newShopcart = new ShopCart();
                    newShopcart.UserId = user.Id;
                    newShopcart.ShopCartItems = new List<ShopCartItem>();
                    foreach(var item in tempShopcart.ShopCartItems)
                    {
                        item.ProductId = item.Product.Id;
                        item.Product = null;
                        newShopcart.ShopCartItems.Add(item);
                    }
                    await _dbcontext.ShopCarts.AddAsync(newShopcart);
                }
                else
                {
                    foreach (var item in tempShopcart.ShopCartItems)
                    {
                        var shopcartItem = shopcart.ShopCartItems.FirstOrDefault(i => i.ProductId == item.Product.Id);
                        if(shopcartItem == null)
                        {
                            item.ProductId = item.Product.Id;
                            item.Product = null;
                            shopcart.ShopCartItems.Add(item);
                        }
                        else
                        {
                            shopcartItem.Count += item.Count;
                            if(shopcartItem.Product.Store < shopcartItem.Count)
                            {
                                shopcartItem.Count = shopcartItem.Product.Store;
                            }
                        }
                    }
                }

                HttpContext.Response.Cookies.Delete("shopcart");

                await _dbcontext.SaveChangesAsync();
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
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "customer"));
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
            [Required(ErrorMessage = "性别是必须的")]
            public string Gender { get; set; }
            [Required(ErrorMessage = "年龄是必须的")]
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
