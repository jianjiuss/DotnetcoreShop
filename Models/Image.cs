using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public ICollection<ProductTitleImage> ProductTitleImages { get; set; }

        [JsonIgnore]
        public ICollection<ProductInfoImage> ProductInfoImages { get; set; }
    }
}
