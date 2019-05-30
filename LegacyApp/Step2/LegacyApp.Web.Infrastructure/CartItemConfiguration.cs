using System.Data.Entity.ModelConfiguration;
using LegacyApp.Web.Domain.Ordering;

namespace LegacyApp.Cart.Infrastructure
{
    public class CartItemConfiguration : EntityTypeConfiguration<CartItem>
    {
        public CartItemConfiguration()
        {
            ToTable("CartItem");

            HasKey(e => e.CartItemId);

            HasRequired(e => e.Cart)
                .WithMany(e => e.Items)
                .HasForeignKey(e => e.CartId);
        }
    }
}