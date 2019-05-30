using System.Linq;
using LegacyApp.Ordering.Domain;

namespace LegacyApp.Ordering.Application
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IGetCartQuery _getCartQuery;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(ICartRepository cartRepository, 
                           IGetCartQuery getCartQuery, 
                           IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _getCartQuery = getCartQuery;
            _unitOfWork = unitOfWork;
        }

        public GetCartQueryResult GetCartById(int cartId)
        {
            var cart = _getCartQuery.GetCartById(cartId);
            return cart;
        }

        public void AddItemToCart(int cartId, int productId)
        {
            var cart = _cartRepository.GetById(cartId);

            if (cart == null)
            {
                throw new System.Exception($"Invalid cart ID: {cartId}");
            }
            else
            {
                var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (item == null)
                {
                    cart.AddItem(productId);
                }
                else
                {
                    item.Quantity++;
                }
            }

            _unitOfWork.Commit();
        }

        public int CreateCart()
        {
            var cart = new Cart();
            _cartRepository.Add(cart);
            _unitOfWork.Commit();
            return cart.CartId;
        }
    }
}
