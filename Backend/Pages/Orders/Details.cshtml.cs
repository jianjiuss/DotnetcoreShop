using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Models;

namespace Backend.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly Backend.Data.MyDbContext _context;

        public DetailsModel(Backend.Data.MyDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders
                .Include(o => o.ShippingAddress)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if(Order == null)
            {
                return NotFound();
            }

            Order.Status = OrderStatus.Deliver;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
