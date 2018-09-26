using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "商品名")]
        public string Name { get; set; }

        [Display(Name = "价格")]
        public double Price { get; set; }

        [Display(Name = "商品缩略图")]
        public string IconImageUrl { get; set; }

        [Display(Name = "库存量")]
        public int Store { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [Display(Name = "分类")]
        public Category Category { get; set; }

        public ICollection<ProductDescription> Descriptions { get; set; }
        
        public ICollection<ProductTitleImage> TitleImages { get; set; }
        
        public ICollection<ProductInfoImage> InfoImages { get; set; }

        [JsonIgnore]
        public ICollection<UserProductCollection> UserProductCollections { get; set; }
    }
}
