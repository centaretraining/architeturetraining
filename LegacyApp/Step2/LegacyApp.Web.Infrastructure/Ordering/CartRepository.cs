using System.Linq;
using LegacyApp.Web.Domain.Ordering;

namespace LegacyApp.Cart.Infrastructure.Ordering
{
    public class CartRepository : ICartRepository
    {
        private readonly LegacyAppDbContext _dbContext;

        public CartRepository(LegacyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Web.Domain.Ordering.Cart cart)
        {
            _dbContext.Carts.Add(cart);
        }

        public Web.Domain.Ordering.Cart GetById(int cartId)
        {
            return _dbContext.Carts.FirstOrDefault(c => c.CartId == cartId);
        }

        public void Delete(Web.Domain.Ordering.Cart cart)
        {
            _dbContext.Carts.Remove(cart);
        }
    }
}
