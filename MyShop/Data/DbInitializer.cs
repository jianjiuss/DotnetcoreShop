using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Data
{
    public class DbInitializer
    {
        public static void Initialize(MyDbContext context)
        {
            if (context.Categories.Any())
            {
                return;   // DB has been seeded
            }

            List<Category> categories = new List<Category>()
            {
                new Category() { Name = "饮料" },
                new Category() { Name = "碳酸饮料" },
                new Category() { Name = "茶饮料" },
                new Category() { Name = "咖啡" },
                new Category() { Name = "奶制品" },
                new Category() { Name = "零食" },
                new Category() { Name = "薯片" },
                new Category() { Name = "饼干" },
                new Category() { Name = "其它" },
                new Category() { Name = "清洁用品" },
                new Category() { Name = "文具" },
                new Category() { Name = "洗发护发" }
            };

            categories[0].ChildCategorys = categories.GetRange(1, 4);
            categories[5].ChildCategorys = categories.GetRange(6, 2);
            categories[8].ChildCategorys = categories.GetRange(9, 3);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            List<Product> products = new List<Product>()
            {
                new Product(){ Name="可乐", Price = 2.5, IconImageUrl = "/images/shopListIcon/cocacola.png", Category = categories[1]},
                new Product(){ Name="雪碧", Price = 3, IconImageUrl = "/images/shopListIcon/sprite.png", Category = categories[1]},
                new Product(){ Name="芬达", Price = 3, IconImageUrl = "/images/shopListIcon/fanda.png", Category = categories[1]},
            };
        }
    }
}
