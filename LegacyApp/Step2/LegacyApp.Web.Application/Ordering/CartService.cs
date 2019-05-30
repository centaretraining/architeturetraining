using System.Linq;
using LegacyApp.Web.Domain;
using LegacyApp.Web.Domain.Ordering;

namespace LegacyApp.Web.Application.Ordering
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddItemToCart(int cartId, int productId)
        {
            var cart = _cartRepository.GetById(cartId);

            if (cart == null)
            {
                throw new System.Exception($"Invalid cart ID: {cartId}");
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
            {
                cart.AddItem(productId);
            }
            else
            {
                item.Quantity++;
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
