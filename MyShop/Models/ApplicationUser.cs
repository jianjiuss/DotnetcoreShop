using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<UserProductCollection> UserProductCollections { get; set; }
    }
}
