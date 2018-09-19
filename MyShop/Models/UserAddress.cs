﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Models
{
    public class UserAddress
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        [Required(ErrorMessage = "所在区域是必须的")]
        public string AddressArea { get; set; }
        [Required(ErrorMessage = "详细地址是必须的")]
        public string AddressDetail { get; set; }
        [Required(ErrorMessage = "邮政编码是必须的")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "手机号码是必须的")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "收货人姓名是必须的")]
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

    }
}
