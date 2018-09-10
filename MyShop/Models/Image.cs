using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<ProductTitleImage> ProductTitleImages { get; set; }

        public ICollection<ProductInfoImage> ProductInfoImages { get; set; }
    }
}
