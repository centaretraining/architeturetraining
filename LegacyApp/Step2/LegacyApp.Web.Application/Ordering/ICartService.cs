namespace LegacyApp.Web.Application.Ordering
{
    public interface ICartService
    {
        void AddItemToCart(int cartId, int productId);

        int CreateCart();
    }
}