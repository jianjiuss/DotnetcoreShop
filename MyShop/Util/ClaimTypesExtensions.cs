using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyShop.Util
{
    public static class ClaimTypesExtensions
    {
        public static string Age { get; } = "age";
        public static string HeadphotoUrl { get; set; } = "headphoto";
    }
}
