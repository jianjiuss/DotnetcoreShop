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
    public class OrderController : ControllerBase
    {
        private readonly MyDbContext _dbcontext;

        public OrderController(MyDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPut]
        public async Task<IActionResult> CreateOrderAsync([FromBody]ShopCart shopCart)
        {
            Order newOrder = new Order();
            newOrder.Status = OrderStatus.Completed;
            newOrder.UserId = shopCart.UserId;
            newOrder.OrderItems = new List<OrderItem>();
            foreach(var item in shopCart.ShopCartItems)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.Count = item.Count;
                orderItem.ProductId = item.ProductId;
                newOrder.OrderItems.Add(orderItem);
            }

            await _dbcontext.Orders.AddAsync(newOrder);
            await _dbcontext.SaveChangesAsync();

            return Ok();
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
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if(order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        public class UpdateStatusJson
        {
            public int StatusCode { get; set; }
        }
    }
}
