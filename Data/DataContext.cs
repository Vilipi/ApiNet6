using Microsoft.EntityFrameworkCore;

namespace ApiNet6.Data {

    public class DataContext : DbContext { // Unit of work
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Product>? Products { get; set; }

    public DbSet<CartItem>? CartItems { get; set; }

    public DbSet<OrderItem>? Orders { get; set; }


    }
 

} 
