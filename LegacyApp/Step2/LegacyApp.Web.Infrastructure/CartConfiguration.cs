using System.Data.Entity.ModelConfiguration;

namespace LegacyApp.Cart.Infrastructure
{
    public class CartConfiguration : EntityTypeConfiguration<Web.Domain.Ordering.Cart>
    {
        public CartConfiguration()
        {
            ToTable("Cart");

            HasKey(e => e.CartId);

            HasMany(e => e.Items);
        }
    }
}