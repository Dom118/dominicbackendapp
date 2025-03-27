using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;
using ProductApi.Models;
using UserApi.Models;
using ShoppingCartApi.Models;
using OrderItemApi.Models;

namespace MinimalX.Data
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<User> Users {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<ShoppingCart> ShoppingCart {get; set;}
        public DbSet<Order> Orders {get; set;}
        public DbSet<OrderItem> OrderItems {get; set;}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:dominic-server.database.windows.net,1433;Initial Catalog=Dominic-DB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default");
        }
    }
}
        