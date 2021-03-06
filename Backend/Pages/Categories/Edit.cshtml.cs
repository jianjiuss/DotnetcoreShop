﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Models;

namespace Backend.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly Backend.Data.MyDbContext _context;

        public EditModel(Backend.Data.MyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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

            var categories = _context.Categories.Where(c => c.ParentId == null && c.Id != id && !c.Name.Equals("未分类")).ToList();
            categories.Insert(0, new Category() { Id = -1, Name = string.Empty });
            ViewData["ParentId"] = new SelectList(categories, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(Category.ParentId != -1 && _context.Categories.Any(c => c.ParentId == Category.Id))
            {
                ModelState.AddModelError("Category.ParentId", "当前类型有下级分类,不能指定上级类型,请先清除下级分类。");

                var categories = _context.Categories.Where(c => c.ParentId == null && c.Id != Category.Id).ToList();
                categories.Insert(0, new Category() { Id = -1, Name = string.Empty });
                ViewData["ParentId"] = new SelectList(categories, "Id", "Name");

                return Page();
            }

            if(Category.ParentId == -1)
            {
                Category.ParentId = null;
            }
            _context.Attach(Category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
