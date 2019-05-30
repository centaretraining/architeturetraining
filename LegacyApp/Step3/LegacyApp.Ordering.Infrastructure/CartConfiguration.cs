using System.Data.Entity.ModelConfiguration;
using LegacyApp.Ordering.Domain;

namespace LegacyApp.Ordering.Infrastructure
{
    public class CartConfiguration : EntityTypeConfiguration<Cart>
    {
        public CartConfiguration()
        {
            ToTable("Cart");

            HasKey(e => e.CartId);

            HasMany(e => e.Items);
        }
    }
}