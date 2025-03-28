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
        public DbSet<UserProfile> Users {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<CartItem> ShoppingCart {get; set;}
        public DbSet<OrderItem> OrderItems {get; set;}
        public DbSet<Order> Orders {get; set;}
        
    }
}
        
