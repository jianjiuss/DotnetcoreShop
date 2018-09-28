using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ShopCartItem
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public bool IsCheck { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int ShopCartId { get; set; }
        [JsonIgnore]
        [ForeignKey("ShopCartId")]
        public ShopCart ShopCart { get; set; }
    }
}
