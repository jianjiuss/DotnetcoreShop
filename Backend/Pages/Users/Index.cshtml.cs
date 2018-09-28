using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Backend.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        public IndexModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PaginatedList<ApplicationUser> Users { get; set; }
        
        
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            Users = await PaginatedList<ApplicationUser>.CreateAsync(
                _dbContext.Users.Where(u => User.HasClaim(ClaimTypes.Role, "customer"))
                , pageIndex.HasValue ? pageIndex.Value : 1);
            return Page();
        }
    }
}