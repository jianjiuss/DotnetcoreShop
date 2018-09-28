using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Backend.Pages.Products
{
    public class PreviewModel : PageModel
    {
        private readonly MyDbContext _dbContext;

        public PreviewModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _dbContext.Products
                .Include(p => p.InfoImages)
                .ThenInclude(i => i.Image)
                .Include(p => p.TitleImages)
                .ThenInclude(i => i.Image)
                .Include(p => p.Descriptions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}