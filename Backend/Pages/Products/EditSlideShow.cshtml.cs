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
    public class EditSlideShowModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        public EditSlideShowModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Product Product { get; set; }
        [BindProperty]
        public Image TitleImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var product = await _dbContext.Products
                .Include(p => p.TitleImages)
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
                .Include(p => p.TitleImages)
                .ThenInclude(i => i.Image)
                .FirstOrDefaultAsync(p => p.Id == productId);

            product.TitleImages.Add(new ProductTitleImage() { Image = TitleImage });
            await _dbContext.SaveChangesAsync();

            Product = product;

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveAsync([FromForm]int productId, int imageId)
        {
            var product = await _dbContext.Products
                .Include(p => p.TitleImages)
                .ThenInclude(i => i.Image)
                .FirstOrDefaultAsync(p => p.Id == productId);

            var productTitleImage = await _dbContext.ProductTitleImages
                .Include(p => p.Image)
                .FirstOrDefaultAsync(p => p.ImageId == imageId && p.ProductId == productId);

            _dbContext.ProductTitleImages.Remove(productTitleImage);
            _dbContext.Images.Remove(productTitleImage.Image);

            await _dbContext.SaveChangesAsync();

            Product = product;

            return Page();
        }
    }
}