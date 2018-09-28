using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ProductController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult> GetByCategoryAsync(int categoryId)
        {
            return Ok(await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync());
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult> GetAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Descriptions)
                .Include(p => p.InfoImages).ThenInclude(i => i.Image)
                .Include(p => p.TitleImages).ThenInclude(i => i.Image)
                .FirstOrDefaultAsync(p => p.Id == productId);

            return Ok(product);
        }
    }
}
