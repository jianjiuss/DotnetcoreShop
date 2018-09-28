using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Models;

namespace Backend.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly Backend.Data.MyDbContext _context;

        public DeleteModel(Backend.Data.MyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; }
        [BindProperty]
        public bool IsConfirm { get; set; }
        public string ConcurrencyErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? isAskToConfirm)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories
                .Include(c => c.ParentCategory).FirstOrDefaultAsync(m => m.Id == id);

            if (Category == null)
            {
                return NotFound();
            }

            if(isAskToConfirm.HasValue && isAskToConfirm.Value)
            {
                ConcurrencyErrorMessage = "当前类型还有下级类型，确定是否删除该类型与该类型的下级类型？";
                IsConfirm = isAskToConfirm.Value;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.Include(c => c.ChildCategorys).FirstOrDefaultAsync(c => c.Id == id);

            if (Category == null)
            {
                return NotFound();
            }

            if(Category.ChildCategorys.Count != 0 && !IsConfirm)
            {
                return RedirectToPage("./Delete", new { IsAskToConfirm = true, Id = id});
            }

            foreach(var item in Category.ChildCategorys)
            {
                await ChangeProductToDefaultCategory(item);
            }
            _context.Categories.RemoveRange(Category.ChildCategorys);

            await ChangeProductToDefaultCategory(Category);
            _context.Categories.Remove(Category);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task ChangeProductToDefaultCategory(Category category)
        {
            var products = await _context.Products.Where(p => p.CategoryId == category.Id).ToListAsync();
            foreach(var product in products)
            {
                product.CategoryId = _context.Categories.First(c => c.Name.Equals("未分类")).Id;
            }
        }
    }
}
