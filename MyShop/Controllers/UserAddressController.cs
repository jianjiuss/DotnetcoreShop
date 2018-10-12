using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "customer")]
    public class UserAddressController :ControllerBase
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly MyDbContext _dbContext;

        public UserAddressController(
            UserManager<ApplicationUser> userManager,
            MyDbContext dbContext)
        {
            _usermanager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet("default")]
        public async Task<IActionResult> GetDefaultAsync()
        {
            var user = await GetUserAndLoadAddressAsync();

            var defaultAddress = await _dbContext.UserAddresses
                .FirstOrDefaultAsync(a => a.IsDefault && a.User == user);

            if(defaultAddress == null)
            {
                return BadRequest("没有设置默认收货地址");
            }

            return Ok(defaultAddress);
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> GetAsync(int? id)
        {
            var user = await GetUserAndLoadAddressAsync();

            if(id.HasValue)
            {
                var address = user.UserAddresses.FirstOrDefault(a => a.Id == id);
                if(address != null)
                {
                    return Ok(address);
                }
                else
                {
                    return NotFound("没有找到该用户地址信息");
                }
            }
            else
            {
                return Ok(user.UserAddresses);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> EditAsync(int id, [FromBody] UserAddress editAddressJson)
        {
            if(!ModelState.IsValid)
            {
                var errorMessage = ModelState.First(s => s.Value.ValidationState == ModelValidationState.Invalid).Value.Errors.First().ErrorMessage;
                return BadRequest(errorMessage);
            }
                
            var user = await GetUserAndLoadAddressAsync();

            var address = user.UserAddresses.FirstOrDefault(a => a.Id == id);
            if(address == null)
            {
                return NotFound("没有找到该用户地址信息");
            }

            if(editAddressJson.IsDefault)
            {
                foreach(var item in user.UserAddresses)
                {
                    if(item.Id != editAddressJson.Id)
                    {
                        item.IsDefault = false;
                    }
                }
            }

            _dbContext.Entry(address).State = EntityState.Detached;
            editAddressJson.Id = address.Id;
            editAddressJson.UserId = address.UserId;
            _dbContext.Entry(editAddressJson).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> AddAsync([FromBody] UserAddress addAddressJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.First().Value.Errors.First().ErrorMessage);
            }

            var user = await GetUserAndLoadAddressAsync();

            if(user.UserAddresses.Count == 0)
            {
                addAddressJson.IsDefault = true;
            }
            addAddressJson.User = user;
            await _dbContext.UserAddresses.AddAsync(addAddressJson);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            var user = await GetUserAndLoadAddressAsync();
            var address = user.UserAddresses.FirstOrDefault(a => a.Id == id);
            if(address == null)
            {
                return NotFound("没有该用户地址信息");
            }

            _dbContext.UserAddresses.Remove(address);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        private async Task<ApplicationUser> GetUserAndLoadAddressAsync()
        {
            var user = await _usermanager.GetUserAsync(User);
            await _dbContext.Entry<ApplicationUser>(user)
                .Collection(u => u.UserAddresses)
                .LoadAsync();
            return user;
        }
    }
}
