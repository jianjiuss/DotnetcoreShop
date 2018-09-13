using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyShop.Models;
using MyShop.Util;

namespace MyShop.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User CurrentUser { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            HttpContext.Session.Set("user", CurrentUser.Name);
        }
    }
}