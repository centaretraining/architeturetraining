namespace LegacyApp.Web.Domain.Ordering
{
    public class GetCartResponse
    {
        public int CartId { get; set; }

        public GetCartItemModel[] Items { get; set; }

        public decimal Total { get; set; }
    }
}
