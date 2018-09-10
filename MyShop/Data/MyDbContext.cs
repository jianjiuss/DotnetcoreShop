using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<ProductDescription>().ToTable("ProductDescription");
            modelBuilder.Entity<ProductTitleImage>().ToTable("ProductTitleImage");
            modelBuilder.Entity<ProductInfoImage>().ToTable("ProductInfoImage");

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
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<ProductDescription> ProductDescriptions { get; set; }

        public DbSet<ProductTitleImage> ProductImages { get; set; }
    }
}
