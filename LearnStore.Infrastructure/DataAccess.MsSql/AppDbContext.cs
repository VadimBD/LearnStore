
namespace LearnStore.Infrastructure.DataAccess.MsSql
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Domain.Entities.Customer> Customers { get; set; }
        public DbSet<Domain.Entities.Order> Orders { get; set; }
        public DbSet<Domain.Entities.Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Domain.Entities.Seller> Sellers { get; set; }

        public DbSet<Domain.Entities.ProductCategory> ProductCategories { get; set; }

        public DbSet<Domain.Entities.Author > Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        { 
            base.OnModelCreating(builder);
            builder.Entity<Customer>().HasMany(c=>c.PurchasedProducts).WithMany();
            builder.Entity<Order>().HasMany(o=>o.Items).WithOne().HasForeignKey("OrderId").OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Order>().HasMany(o => o.Payments);
        }
    }
}
