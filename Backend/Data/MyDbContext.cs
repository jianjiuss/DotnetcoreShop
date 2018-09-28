using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data
{
    public class MyDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<ProductDescription>().ToTable("Product_Description");
            modelBuilder.Entity<ProductTitleImage>().ToTable("Product_TitleImage");
            modelBuilder.Entity<ProductInfoImage>().ToTable("Product_InfoImage");
            modelBuilder.Entity<ShopCart>().ToTable("ShopCart");
            modelBuilder.Entity<ShopCartItem>().ToTable("ShopCart_Item");
            modelBuilder.Entity<UserProductCollection>().ToTable("User_ProductCollection");
            modelBuilder.Entity<UserAddress>().ToTable("User_Address");

            modelBuilder.Entity<ProductTitleImage>().HasKey(p => new { p.ImageId, p.ProductId });

            modelBuilder.Entity<ProductTitleImage>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.TitleImages)
            .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductTitleImage>()
                .HasOne(pi => pi.Image)
                .WithMany(t => t.ProductTitleImages)
                .HasForeignKey(pi => pi.ImageId);

            modelBuilder.Entity<ProductInfoImage>().HasKey(p => new { p.ImageId, p.ProductId });

            modelBuilder.Entity<ProductInfoImage>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.InfoImages)
            .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<ProductInfoImage>()
                .HasOne(pi => pi.Image)
                .WithMany(t => t.ProductInfoImages)
                .HasForeignKey(pi => pi.ImageId);

            modelBuilder.Entity<UserProductCollection>().HasKey(p => new { p.ProductId, p.UserId });

            modelBuilder.Entity<UserProductCollection>()
                .HasOne(p => p.Product)
                .WithMany(u => u.UserProductCollections)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<UserProductCollection>()
                .HasOne(p => p.User)
                .WithMany(u => u.UserProductCollections)
                .HasForeignKey(p => p.UserId);
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<ProductDescription> ProductDescriptions { get; set; }

        public DbSet<ProductTitleImage> ProductTitleImages { get; set; }

        public DbSet<ProductInfoImage> ProductInfoImages { get; set; }

        public DbSet<ShopCart> ShopCarts { get; set; }

        public DbSet<ShopCartItem> ShopCartItems { get; set; }

        public DbSet<UserProductCollection> UserProductCollections { get; set; }

        public DbSet<UserAddress> UserAddresses { get; set; }
    }
}
