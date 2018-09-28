using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<UserAddress> UserAddresses { get; set; }
        public ICollection<UserProductCollection> UserProductCollections { get; set; }
    }
}
