﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Category
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Display(Name="类型名称")]
        [Required(ErrorMessage = "类型名称是必须的")]
        public string Name { get; set; }

        public Category ParentCategory { get; set; }

        [ForeignKey("ParentId")]
        [JsonIgnore]
        public ICollection<Category> ChildCategorys { get; set; }
    }
}
