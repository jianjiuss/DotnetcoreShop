using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using MyShop.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpGet("current")]
        public ActionResult<User> Get()
        {
            User user = new User();
            user.Name = HttpContext.Session.GetString("user");
            return Json(user);
        }
    }
}
