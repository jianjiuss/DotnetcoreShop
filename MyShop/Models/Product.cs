using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string IconImageUrl { get; set; }

        public int Store { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<ProductDescription> Descriptions { get; set; }
        
        public ICollection<ProductTitleImage> TitleImages { get; set; }
        
        public ICollection<ProductInfoImage> InfoImages { get; set; }
    }
}
