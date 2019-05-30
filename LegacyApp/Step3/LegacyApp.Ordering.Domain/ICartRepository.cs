namespace LegacyApp.Ordering.Domain
{
    public interface ICartRepository
    {
        void Add(Cart cart);

        Cart GetById(int cartId);

        void Delete(Cart cart);
    }
}