using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<List<Product>> GetAll()
        {
            return _context.Products.ToList();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult> GetByCategory(int categoryId)
        {
            return Ok(await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync());
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult> Get(int productId)
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
