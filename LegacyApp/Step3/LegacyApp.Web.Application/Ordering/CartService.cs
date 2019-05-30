using System.Linq;
using System.Threading.Tasks;
using LegacyApp.Ordering.Domain;
using LegacyApp.Web.Domain.Ordering;

namespace LegacyApp.Web.Application.Ordering
{
    public class CartService : ICartService
    {
        private readonly IOrderApiClient _orderApiClient;

        public CartService(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }

        public async Task<GetCartQueryResult> GetCartById(int cartId)
        {
            var cart = await _orderApiClient.GetCartById(cartId);
            return new GetCartQueryResult()
            {
                CartId = cart.CartId,
                Items = cart.Items.Select(i => new CartItemResult()
                {
                    CartItemId = i.CartItemId,
                    Price = i.Price,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity
                }).ToArray()
            };
        }

        public async Task<int> CreateCart()
        {
            var cart = await _orderApiClient.CreateCart();
            return cart.CartId;
        }

        public async Task AddCartItem(int cartId, int productId)
        {
            var cart = await _orderApiClient.GetCartById(cartId);

            if (cart == null)
            {
                throw new System.Exception($"Invalid cart ID: {cartId}");
            }

            await _orderApiClient.CreateItem(cartId, productId);
        }
    }
}
