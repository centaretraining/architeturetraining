namespace LegacyApp.Ordering.Domain
{
    public class CartItem
    {
        protected CartItem()
        {
        }

        public CartItem(Cart cart, int productId)
        {
            CartId = cart.CartId;
            ProductId = productId;
        }

        public int CartItemId { get; protected set; }

        public int CartId { get; protected set; }

        public Cart Cart { get; protected set; }

        public int ProductId { get; protected set; }

        public int Quantity { get; set; }
    }
}