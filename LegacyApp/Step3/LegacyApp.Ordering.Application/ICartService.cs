using LegacyApp.Ordering.Domain;

namespace LegacyApp.Ordering.Application
{
    public interface ICartService
    {
        void AddItemToCart(int cartId, int productId);

        GetCartQueryResult GetCartById(int cartId);

        int CreateCart();
    }
}