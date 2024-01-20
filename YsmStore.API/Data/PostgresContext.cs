using Microsoft.EntityFrameworkCore;
using YsmStore.API.Models;

namespace YsmStore.API.Data
{
    public class PostgresContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Option> Options { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<ProductInCart> ProductInCarts { get; set; }

        public PostgresContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PostgresContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString is not null)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }

            // Добвалено, чтобы устранить ошибку вставки даты времени
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Option>().HasKey(o => new { o.ProductTitle, o.Number });
            modelBuilder.Entity<Customer>().HasIndex(c => c.Login).IsUnique();
            modelBuilder.Entity<OrderedProduct>().HasKey(op => new { op.OrderId, op.ProductId });
            modelBuilder.Entity<ProductInCart>().HasKey(pk => new { pk.CustomerId, pk.ProductId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
