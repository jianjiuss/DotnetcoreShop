using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public Category ParentCategory { get; set; }

        [ForeignKey("ParentId")]
        [JsonIgnore]
        public ICollection<Category> ChildCategorys { get; set; }
    }
}
