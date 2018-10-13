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
    public class IndexModel : PageModel
    {
        private readonly Backend.Data.MyDbContext _context;

        public IndexModel(Backend.Data.MyDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            Order = await _context.Orders
                .Include(o => o.ShippingAddress)
                .Include(o => o.User).ToListAsync();
        }
    }
}
