using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class UserProductCollection
    {
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}
