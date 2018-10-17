using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Security.Claims;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "customer")]
    public class ShopCartController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShopCartController(MyDbContext context
            ,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            if(!User.Identity.IsAuthenticated)
            {
                ShopCart tempShopCart = GetShopCartInCookies();
                HttpContext.Response.Cookies.Append("shopcart", JsonConvert.SerializeObject(tempShopCart), new CookieOptions() { MaxAge = TimeSpan.FromDays(7) });
                return Ok(tempShopCart);
            }

            var shopCart = await GetShopCartAsync();

            if(shopCart == null)
            {
                shopCart = new ShopCart() { User = await _userManager.GetUserAsync(User) };
                await _context.ShopCarts.AddAsync(shopCart);
                await _context.SaveChangesAsync();
            }

            return Ok(shopCart);
        }

        [HttpPut]
        public async Task<IActionResult> AddProductAsync([FromBody] AddProductJson addProductJson)
        {
            if(addProductJson.Numb <= 0)
            {
                return BadRequest("请确定商品商量");
            }

            ShopCart shopCart;
            if(User.Identity.IsAuthenticated)
            {
                shopCart = await GetShopCartAsync();

                if (shopCart == null)
                {
                    shopCart = new ShopCart();
                    shopCart.ShopCartItems = new HashSet<ShopCartItem>();
                }
            }
            else
            {
                shopCart = GetShopCartInCookies();
            }
                

            var shopCartItem = shopCart.ShopCartItems
                .FirstOrDefault(i => i.Product.Id == addProductJson.ProductId);

            if (shopCartItem == null)
            {
                var product = await _context.Products
                    .FindAsync(addProductJson.ProductId);
                if (product == null)
                {
                    return NotFound("没有找到此商品");
                }
                var cartItem = new ShopCartItem() { Product = product, Count = addProductJson.Numb, ShopCart = shopCart, IsCheck = true};
                if(cartItem.Count > product.Store)
                {
                    return BadRequest("库存不足");
                }
                shopCart.ShopCartItems.Add(cartItem);
            }
            else
            {
                shopCartItem.Count += addProductJson.Numb;
                if(shopCartItem.Count > shopCartItem.Product.Store)
                {
                    return BadRequest("购物车购买数量已经超出库存数量");
                }
            }

            if (User.Identity.IsAuthenticated)
            {
                _context.SaveChanges();
            }
            else
            {
                HttpContext.Response.Cookies.Append("shopcart", JsonConvert.SerializeObject(shopCart), new CookieOptions() { MaxAge = TimeSpan.FromDays(7) });
            }
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveCartItemAsync(int productId)
        {
            ShopCart shopCart;
            if (User.Identity.IsAuthenticated)
            {
                shopCart = await GetShopCartAsync();

                if (shopCart == null)
                {
                    shopCart = new ShopCart();
                    shopCart.ShopCartItems = new HashSet<ShopCartItem>();
                }
            }
            else
            {
                shopCart = GetShopCartInCookies();
            }

            var item = shopCart.ShopCartItems.FirstOrDefault(i => i.Product.Id == productId);
            if (item == null)
            {
                return NotFound();
            }

            shopCart.ShopCartItems.Remove(item);

            if (User.Identity.IsAuthenticated)
            {
                await _context.SaveChangesAsync();
            }
            else
            {
                HttpContext.Response.Cookies.Append("shopcart", JsonConvert.SerializeObject(shopCart), new CookieOptions() { MaxAge = TimeSpan.FromDays(7) });
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCartItemAsync([FromBody] UpdateItemJson updateItemJson)
        {
            ShopCart shopCart;
            if (User.Identity.IsAuthenticated)
            {
                shopCart = await GetShopCartAsync();

                if (shopCart == null)
                {
                    shopCart = new ShopCart();
                    shopCart.ShopCartItems = new HashSet<ShopCartItem>();
                }
            }
            else
            {
                shopCart = GetShopCartInCookies();
            }

            var item = shopCart.ShopCartItems.FirstOrDefault(i => i.Product.Id == updateItemJson.ProductId);
            if(item == null)
            {
                return NotFound();
            }

            item.Count = updateItemJson.Numb;
            item.IsCheck = updateItemJson.IsCheck;
            if(item.Count <= 0)
            {
                return BadRequest("购买数量不能少于1");
            }
            if(item.Count > item.Product.Store)
            {
                return BadRequest("库存不足");
            }

            if (User.Identity.IsAuthenticated)
            {
                await _context.SaveChangesAsync();
            }
            else
            {
                HttpContext.Response.Cookies.Append("shopcart", JsonConvert.SerializeObject(shopCart), new CookieOptions() { MaxAge = TimeSpan.FromDays(7) });
            }
            return Ok();
        }

        private ShopCart GetShopCartInCookies()
        {
            if(HttpContext.Request.Cookies.Any(c => c.Key.Equals("shopcart")))
            {
                var shopcartCookie = HttpContext.Request.Cookies.First(c => c.Key.Equals("shopcart"));
                return JsonConvert.DeserializeObject<ShopCart>(shopcartCookie.Value);
            }
            else
            {
                ShopCart shopcart = new ShopCart();
                shopcart.ShopCartItems = new List<ShopCartItem>();
                return shopcart;
            }
        }

        private async Task<ShopCart> GetShopCartAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var shopCart = await _context.ShopCarts
                .Include(s => s.ShopCartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.User == user);
            return shopCart;
        }

        public class AddProductJson
        {
            public int ProductId { get; set; }
            public int Numb { get; set; }
        }

        public class UpdateItemJson
        {
            public int ProductId { get; set; }
            public int Numb { get; set; }
            public bool IsCheck { get; set; }
        }
    }
}
