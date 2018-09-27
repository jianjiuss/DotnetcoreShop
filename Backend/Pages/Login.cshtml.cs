using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Backend.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginInfo User { get; set; }

        public IActionResult OnGet()
        {
            if(_signInManager.IsSignedIn(HttpContext.User))
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _signInManager.PasswordSignInAsync(User.Username, User.Password, User.IsRemeber, lockoutOnFailure: true);
            if(result.Succeeded)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }
        }

        public class LoginInfo
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool IsRemeber { get; set; }
        }
    }
}