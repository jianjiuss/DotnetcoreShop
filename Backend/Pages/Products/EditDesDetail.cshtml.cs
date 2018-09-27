using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Backend.Pages.Products
{
    public class EditDesDetailModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        public EditDesDetailModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Product Product { get; set; }

        [BindProperty]
        public ProductDescription Des { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(!id.HasValue)
            {
                return NotFound();
            }

            var product = await _dbContext.Products
                .Include(p => p.Descriptions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if(product == null)
            {
                return NotFound();
            }
            
            Product = product;
            return Page();
        }

        public async Task OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return;
            }

            await _dbContext.ProductDescriptions.AddAsync(Des);
            await _dbContext.SaveChangesAsync();

            Product = await _dbContext.Products
                .Include(p => p.Descriptions)
                .FirstOrDefaultAsync(p => p.Id == Des.ProductId);
        }

        public async Task<IActionResult> OnPostRemoveAsync(int? id)
        {
            if(!id.HasValue)
            {
                return NotFound();
            }

            var des = await _dbContext.ProductDescriptions
                .Include(d => d.Product)
                .ThenInclude(p => p.Descriptions)
                .FirstOrDefaultAsync(d => d.Id == id);

            if(des == null)
            {
                return NotFound();
            }

            Product = des.Product;
            Product.Descriptions.Remove(des);
            await _dbContext.SaveChangesAsync();

            return Page();
        }
    }
}