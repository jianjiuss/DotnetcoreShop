using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Models
{
    public class ProductInfoImage
    { 
        public int ProductId { get; set; }
        public int ImageId { get; set; }
        public Product Product { get; set; }
        public Image Image { get; set; }
    }
}
