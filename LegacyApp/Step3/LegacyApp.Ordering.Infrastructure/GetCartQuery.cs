using System.Linq;
using LegacyApp.Ordering.Domain;

namespace LegacyApp.Ordering.Infrastructure
{
    public class GetCartQuery : IGetCartQuery
    {
        private readonly LegacyAppDbContext _dbContext;

        public GetCartQuery(LegacyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetCartQueryResult GetCartById(int cartId)
        {
            var cartSet = _dbContext.Set<Cart>();
            var cartItemSet = _dbContext.Set<CartItem>();
            var productSet = _dbContext.Set<Product>();

            var cartQuery = from c in cartSet
                            where c.CartId == cartId
                            select c.CartId;
            if (!cartQuery.Any())
            {
                return null;
            }

            var itemQuery = from i in cartItemSet
                    join p in productSet on i.ProductId equals p.ProductId
                    where i.CartId == cartId
                    select new CartItemResult()
                    {
                        ProductId = p.ProductId,
                        Price = p.Price,
                        Quantity = i.Quantity,
                        ProductName = p.Name
                    };

            var result = new GetCartQueryResult()
            {
                CartId = cartId,
                Items = itemQuery.ToArray()
            };

            return result;
        }
    }
}
