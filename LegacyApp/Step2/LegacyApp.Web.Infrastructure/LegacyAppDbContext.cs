using System.Configuration;
using System.Data.Entity;

namespace LegacyApp.Cart.Infrastructure
{
    public class LegacyAppDbContext : DbContext
    {
        public LegacyAppDbContext() : this(ConfigurationManager.ConnectionStrings["Database"].ConnectionString)
        {
        }

        public LegacyAppDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Web.Domain.Ordering.Cart> Carts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CartConfiguration());
            modelBuilder.Configurations.Add(new CartItemConfiguration());
        }
    }
}
