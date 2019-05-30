namespace LegacyApp.Web.Application.Cart
{
    public interface ICartService
    {
        void AddItemToCart(int cartId, int productId);

        int CreateCart();
    }
}