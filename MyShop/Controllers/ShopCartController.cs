﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyShop.Models;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShopCartController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ShopCartController(MyDbContext context
            ,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var shopCart = await GetShopCartAsync();

            if(shopCart == null)
            {
                shopCart = new ShopCart() { UserId = _userManager.GetUserId(User) };
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

            var shopCart = await GetShopCartAsync();

            if(shopCart == null)
            {
                shopCart = new ShopCart();
                shopCart.ShopCartItems = new HashSet<ShopCartItem>();
            }

            var shopCartItem = shopCart.ShopCartItems
                .FirstOrDefault(i => i.ProductId == addProductJson.ProductId);
            if(shopCartItem == null)
            {
                var product = await _context.Products
                    .FindAsync(addProductJson.ProductId);
                if(product == null)
                {
                    return NotFound("没有找到此商品");
                }
                var cartItem = new ShopCartItem() { Product = product, Count = addProductJson.Numb, ShopCart = shopCart, IsCheck = true};
                shopCart.ShopCartItems.Add(cartItem);
            }
            else
            {
                shopCartItem.Count += addProductJson.Numb;
            }

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> RemoveCartItemAsync(int cartItemId)
        {
            var shopCart = await GetShopCartAsync();

            if (shopCart == null)
            {
                return NotFound();
            }

            var item = shopCart.ShopCartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (item == null)
            {
                return NotFound();
            }

            shopCart.ShopCartItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCartItemAsync([FromBody] UpdateItemJson updateItemJson)
        {
            var shopCart = await GetShopCartAsync();
            
            if(shopCart == null)
            {
                return NotFound();
            }

            var item = shopCart.ShopCartItems.FirstOrDefault(i => i.Id == updateItemJson.CartItemId);
            if(item == null)
            {
                return NotFound();
            }

            item.Count = updateItemJson.Numb;
            item.IsCheck = updateItemJson.IsCheck;
            if(item.Count <= 0)
            {
                return BadRequest();
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<ShopCart> GetShopCartAsync()
        {
            var userId = _userManager.GetUserId(User);
            var shopCart = await _context.ShopCarts
                .Include(s => s.ShopCartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.UserId == userId);
            return shopCart;
        }

        public class AddProductJson
        {
            public int ProductId { get; set; }
            public int Numb { get; set; }
        }

        public class UpdateItemJson
        {
            public int CartItemId { get; set; }
            public int Numb { get; set; }
            public bool IsCheck { get; set; }
        }
    }
}