using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReSharpersCooperation.Models;
using ReSharpersCooperation.Models.Category;

namespace ReSharpersCooperation.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Product {get;set;}
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Cart_Item> Cart_Item { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Category_Product> Category_Product { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category_Product>()
                .HasKey(cp => new { cp.ProductNo, cp.CategoryName });
        }
    }
}
