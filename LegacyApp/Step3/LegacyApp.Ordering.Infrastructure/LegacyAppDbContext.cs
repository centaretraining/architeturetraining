using System.Configuration;
using System.Data.Entity;
using LegacyApp.Ordering.Domain;

namespace LegacyApp.Ordering.Infrastructure
{
    public class LegacyAppDbContext : DbContext
    {
        public LegacyAppDbContext() : this(ConfigurationManager.ConnectionStrings["Database"].ConnectionString)
        {
        }

        public LegacyAppDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CartConfiguration());
            modelBuilder.Configurations.Add(new CartItemConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
        }
    }
}
