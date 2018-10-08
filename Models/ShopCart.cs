using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ShopCart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public double TotalPrice
        {
            get
            {
                return ShopCartItems.Sum(i => (i.Product == null ? 0 : i.Product.Price) * i.Count);
            }
        }
        public ICollection<ShopCartItem> ShopCartItems { get; set; } = new HashSet<ShopCartItem>();

    }
}
