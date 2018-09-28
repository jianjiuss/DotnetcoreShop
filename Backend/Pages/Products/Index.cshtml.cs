using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Models;

namespace Backend.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly Backend.Data.MyDbContext _context;

        public IndexModel(Backend.Data.MyDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Product> Product { get;set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            Product = await PaginatedList<Product>.CreateAsync( _context.Products
                .Include(p => p.Category), pageIndex.HasValue ? pageIndex.Value : 1);
        }
    }
}
