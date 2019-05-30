namespace LegacyApp.Ordering.Domain
{
    public class Product
    {
        protected Product()
        {
        }

        public int ProductId { get; protected set; }

        public string Name { get; protected set; }

        public decimal Price { get; protected set; }
    }
}
