﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ProductTitleImage
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        [JsonIgnore]
        public int ImageId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        public Image Image { get; set; }
    }
}
