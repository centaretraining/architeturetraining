namespace LegacyApp.Ordering.Domain
{
    public class GetCartQueryResult
    {
        public int CartId { get; set; }

        public CartItemResult[] Items { get; set; }
    }
}