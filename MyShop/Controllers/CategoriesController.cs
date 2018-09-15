using Microsoft.AspNetCore.Authorization;
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
    public class CategoriesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CategoriesController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet("childs/{id?}")]
        public async Task<ActionResult> GetAllAsync(int? id)
        {
            if(id.HasValue)
            {
                return Ok(await _context.Categories.Where(c => c.ParentId == id).ToListAsync());
            }
            else
            {
                return Ok(await _context.Categories.Where(c => c.ParentId == null).ToListAsync());
            }
        }
    }
}
