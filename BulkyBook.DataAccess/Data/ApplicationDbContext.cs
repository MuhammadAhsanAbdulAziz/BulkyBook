using BulkyBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess
{
    public class ApplicationDbContext :IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        public virtual DbSet<Category>? categories { get; set; }
        public virtual DbSet<CoverType>? coverTypes { get; set; }
        public virtual DbSet<Product>? Products { get; set; }
        public virtual DbSet<ApplicationUser>? ApplicationUsers { get; set; }
        public virtual DbSet<Company>? Companies { get; set; }
        public virtual DbSet<ShoppingCart>? ShoppingCarts { get; set; }
        public virtual DbSet<OrderHeader>? OrderHeaders { get; set; }
        public virtual DbSet<OrderDetails>? OrderDetails { get; set; }
    }
}

