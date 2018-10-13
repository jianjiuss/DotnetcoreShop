using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Backend.Data;
using Models;

namespace Backend.Pages.Orders
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
        ViewData["ShippingAddressId"] = new SelectList(_context.UserAddresses, "Id", "AddressArea");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}