using System.Data.Entity.ModelConfiguration;
using LegacyApp.Ordering.Domain;

namespace LegacyApp.Ordering.Infrastructure
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            ToTable("Product");

            HasKey(e => e.ProductId);
        }
    }
}