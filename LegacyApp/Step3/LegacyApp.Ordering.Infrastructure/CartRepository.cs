using System.Linq;
using LegacyApp.Ordering.Domain;

namespace LegacyApp.Ordering.Infrastructure
{
    public class CartRepository : ICartRepository
    {
        private readonly LegacyAppDbContext _dbContext;

        public CartRepository(LegacyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Cart cart)
        {
            _dbContext.Carts.Add(cart);
        }

        public Cart GetById(int cartId)
        {
            return _dbContext.Carts.FirstOrDefault(c => c.CartId == cartId);
        }

        public void Delete(Cart cart)
        {
            _dbContext.Carts.Remove(cart);
        }
    }
}
