using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace Backend.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MyDbContext _context;

        public DetailsModel(
            UserManager<ApplicationUser> userManager
            , MyDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public UserInfo AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(user);

            AppUser = new UserInfo();
            AppUser.User = user;
            AppUser.Gender = claims.FirstOrDefault(c => c.Type == ClaimTypes.Gender)?.Value;
            AppUser.Area = claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value;
            AppUser.Age = claims.FirstOrDefault(c => c.Type == "age")?.Value;

            return Page();
        }

        public class UserInfo
        {
            public ApplicationUser User { get; set; }
            public string Gender { get; set; }
            public string Area { get; set; }
            public string Age { get; set; }
        }
    }
}