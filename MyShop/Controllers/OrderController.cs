using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "customer")]
    public class OrderController : ControllerBase
    {
        private readonly MyDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _usermanager;

        public OrderController(MyDbContext dbcontext, UserManager<ApplicationUser> userManager)
        {
            _dbcontext = dbcontext;
            _usermanager = userManager;
        }

        [HttpPut]
        public async Task<IActionResult> CreateOrderAsync([FromBody]CreateOrderJson createOrderJson)
        {
            var myShopCart = await _dbcontext.ShopCarts
                .Include(s => s.ShopCartItems)
                .FirstOrDefaultAsync(s => s.Id == createOrderJson.ShopCart.Id);

            var shopCart = createOrderJson.ShopCart;

            //创建订单
            Order newOrder = new Order();
            newOrder.Status = OrderStatus.Created;
            newOrder.UserId = shopCart.UserId;
            newOrder.Remark = createOrderJson.Remark;
            newOrder.OrderItems = new List<OrderItem>();
            newOrder.ShippingAddressId = createOrderJson.ShippingAddressId;
            foreach(var item in shopCart.ShopCartItems)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.Count = item.Count;
                orderItem.ProductId = item.ProductId;
                newOrder.OrderItems.Add(orderItem);
                
            }

            await _dbcontext.Orders.AddAsync(newOrder);
            await _dbcontext.SaveChangesAsync();


            //成功后移除购物车项
            var removeItems = new List<ShopCartItem>();
            foreach (var item in shopCart.ShopCartItems)
            {
                var removeItem = myShopCart.ShopCartItems.First(i => i.Id == item.Id);
                removeItems.Add(removeItem);
            }
            
            foreach(var item in removeItems)
            {
                myShopCart.ShopCartItems.Remove(item);
            }

            await _dbcontext.SaveChangesAsync();

            //扣除商品库存 TODO

            return Ok(newOrder.Id);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int id,[FromBody]UpdateStatusJson updateStatusJson)
        {
            if(updateStatusJson == null)
            {
                return BadRequest("订单状态不能为空");
            }

            var order = await _dbcontext.Orders
                .FindAsync(id);

            if(order == null)
            {
                return NotFound();
            }

            if(!Enum.IsDefined(typeof(OrderStatus), updateStatusJson.StatusCode))
            {
                return BadRequest("订单状态错误");
            }

            order.Status = (OrderStatus)updateStatusJson.StatusCode;

            await _dbcontext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _dbcontext.Orders
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if(order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("list/{statusCode?}")]
        public async Task<IActionResult> GetOrder(int? statusCode)
        {
            var appuser =  await _usermanager.GetUserAsync(User);

            if(!statusCode.HasValue)
            {
                var orders = await _dbcontext.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                    .Where(o => o.User == appuser)
                    .ToListAsync();

                return Ok(orders);
            }

            if (!Enum.IsDefined(typeof(OrderStatus), statusCode))
            {
                return BadRequest("订单状态错误");
            }

            var statusOrders = await _dbcontext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .Where(o => o.Status == (OrderStatus)statusCode && o.User == appuser)
                .ToListAsync();

            return Ok(statusOrders);
        }

        public class UpdateStatusJson
        {
            public int StatusCode { get; set; }
        }

        public class CreateOrderJson
        {
            public ShopCart ShopCart { get; set; }
            public int ShippingAddressId { get; set; }

            public string Remark { get; set; }
        }
    }
}
