using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Backend.Data;
using Backend.Models;

namespace Backend.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly Backend.Data.MyDbContext _context;

        public CreateModel(Backend.Data.MyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var categories = _context.Categories.Where(c => c.ParentId == null).ToList();
            categories.Insert(0, new Category() { Id = -1, Name = string.Empty });
            ViewData["ParentId"] = new SelectList(categories, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(Category.ParentId == -1)
            {
                Category.ParentId = null;
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}