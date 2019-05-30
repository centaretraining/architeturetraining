namespace LegacyApp.Ordering.Api.Models
{
    public class GetCartItemModel
    {
        public int CartItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}