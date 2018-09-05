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
                new Category() { Name = "茶饮料" }
            };

            categories[0].ChildCategorys = new List<Category>(categories.GetRange(1, 2));
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
    }
}
