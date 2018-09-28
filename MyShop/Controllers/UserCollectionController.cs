using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class UserCollectionController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCollectionController(MyDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddAsync(int id)
        {
            var user = await GetUserAndLoadCollectionsAsync();

            if(user.UserProductCollections.Any(c => c.ProductId == id))
            {
                return BadRequest("已经收藏该商品");
            }

            if(!_context.Products.Any(p => p.Id == id))
            {
                return NotFound();
            }

            user.UserProductCollections.Add(new UserProductCollection() { UserId = user.Id, ProductId = id });
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            var user = await GetUserAndLoadCollectionsAsync();

            var collection = user.UserProductCollections.FirstOrDefault(c => c.ProductId == id);

            if(collection == null)
            {
                return NotFound("找不到该收藏商品");
            }

            user.UserProductCollections.Remove(collection);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> GetAsync(int? id)
        {
            var user = await GetUserAndLoadCollectionsAsync();

            if(id.HasValue)
            {
                var collection = user.UserProductCollections.FirstOrDefault(c => c.ProductId == id);
                if(collection != null)
                {
                    await _context.Entry<UserProductCollection>(collection)
                        .Reference(c => c.Product)
                        .LoadAsync();

                    return Ok(collection.Product);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                var products = user.UserProductCollections.Select(pc => {
                    _context.Entry<UserProductCollection>(pc)
                        .Reference(c => c.Product)
                        .Load();
                    return pc.Product;
                }).ToList();
                return Ok(products);
            }
        }

        private async Task<ApplicationUser> GetUserAndLoadCollectionsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            await _context.Entry<ApplicationUser>(user)
                .Collection(u => u.UserProductCollections)
                .LoadAsync();
            return user;
        }
    }
}
