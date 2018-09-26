﻿using MyShop.Models;
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
                new Category() { Name = "洗发护发" },
                new Category() {Name = "未分类"}
            };

            categories[0].ChildCategorys = categories.GetRange(1, 4);
            categories[5].ChildCategorys = categories.GetRange(6, 2);
            categories[8].ChildCategorys = categories.GetRange(9, 3);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            List<Image> images = new List<Image>()
            {
                new Image(){ImageUrl = "/images/productTitle/cocacola_title.jpg"},
                new Image(){ImageUrl = "/images/productInfo/testImageInfo.png"},
                new Image(){ImageUrl = "/images/productInfo/testImageInfo.png"},
                new Image(){ImageUrl = "/images/productInfo/testImageInfo.png"},
                new Image(){ImageUrl = "/images/productInfo/testImageInfo.png"}
            };
            context.Images.AddRange(images);
            context.SaveChanges();


            List<Product> products = new List<Product>()
            {
                new Product(){ Name="可乐", Price = 2.5, Store = 15, IconImageUrl = "/images/productIcon/cocacola.png", Category = categories[1]},
                new Product(){ Name="雪碧", Price = 3, Store = 17, IconImageUrl = "/images/productIcon/sprite.png", Category = categories[1]},
                new Product(){ Name="芬达", Price = 3, Store = 20, IconImageUrl = "/images/productIcon/fanda.png", Category = categories[1]},
                new Product(){ Name="阿萨姆奶茶", Price = 4, Store = 22, IconImageUrl = "/images/productIcon/asamu.png", Category = categories[2]},
                new Product(){Name ="雀巢咖啡（瓶装）", Price = 7, Store = 23, IconImageUrl = "/images/productIcon/quechaoping.png", Category = categories[3]},
                new Product(){Name ="雀巢咖啡（罐装）", Price = 6, Store = 10, IconImageUrl = "/images/productIcon/quechaoguan.png", Category = categories[3]},
                new Product(){Name ="蒙牛", Price = 7, Store = 28, IconImageUrl = "/images/productIcon/mengniu.png", Category = categories[4]},
                new Product(){Name ="乐视薯片", Price = 5, Store = 3, IconImageUrl = "/images/productIcon/leshishupian.png", Category = categories[6]},
                new Product(){Name ="奥利奥", Price = 7, Store = 2, IconImageUrl = "/images/productIcon/aoliao.png", Category = categories[7]},
                new Product(){Name ="扫把", Price = 10, Store = 1, IconImageUrl = "/images/productIcon/saoba.png", Category = categories[9]},
                new Product(){Name ="原子笔", Price = 2, Store = 0, IconImageUrl = "/images/productIcon/yuanzibi.png", Category = categories[10]},
                new Product(){Name ="飘柔", Price = 15, Store = 10, IconImageUrl = "/images/productIcon/piaorou.png", Category = categories[11]}
            };

            products[0].Descriptions = new List<ProductDescription>()
            {
                new ProductDescription(){Title = "容量", Content = "150ML"},
                new ProductDescription(){Title = "能量", Content = "100千焦"},
                new ProductDescription(){Title = "纳", Content = "16毫克"}
            };

            products[0].TitleImages = new List<ProductTitleImage>
            {
                new ProductTitleImage(){Product = products[0], Image = images[0]}
            };

            products[0].InfoImages = new List<ProductInfoImage>
            {
                new ProductInfoImage(){Product = products[0], Image = images[1]},
                new ProductInfoImage(){Product = products[0], Image = images[2]},
                new ProductInfoImage(){Product = products[0], Image = images[3]},
                new ProductInfoImage(){Product = products[0], Image = images[4]}
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
