using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Models
{
    public class Image
    {
        public int Id { get; set; }

        public string ImageUri{ get; set; }

        public string Server { get; set; } = Util.ServiceLocation.ImageService;

        public string ImageUrl { get { return Server + ImageUri; } }

        [JsonIgnore]
        public ICollection<ProductTitleImage> ProductTitleImages { get; set; }

        [JsonIgnore]
        public ICollection<ProductInfoImage> ProductInfoImages { get; set; }
    }
}
