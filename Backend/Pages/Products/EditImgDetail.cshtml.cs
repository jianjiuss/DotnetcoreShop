using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Backend.Pages.Products
{
    public class EditImgDetailModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        public EditImgDetailModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Product Product { get; set; }
        [BindProperty]
        public Image InfoImage { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var product = await _dbContext.Products
                .Include(p => p.InfoImages)
                .ThenInclude(i => i.Image)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync([FromForm]int productId)
        {
            var product = await _dbContext.Products
                .Include(p => p.InfoImages)
                .ThenInclude(i => i.Image)
                .FirstOrDefaultAsync(p => p.Id == productId);

            product.InfoImages.Add(new ProductInfoImage() { Image = InfoImage });
            await _dbContext.SaveChangesAsync();

            Product = product;

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync([FromForm]int productId, int imageId)
        {
            var product = await _dbContext.Products
                .Include(p => p.InfoImages)
                .ThenInclude(i => i.Image)
                .FirstOrDefaultAsync(p => p.Id == productId);

            var productInfoImage = await _dbContext.ProductInfoImages
                .Include(p => p.Image)
                .FirstOrDefaultAsync(p => p.ImageId == imageId && p.ProductId == productId);

            _dbContext.ProductInfoImages.Remove(productInfoImage);
            _dbContext.Images.Remove(productInfoImage.Image);

            await _dbContext.SaveChangesAsync();

            Product = product;

            return Page();
        }
    }
}