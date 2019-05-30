using System.Linq;
using System.Web.Http;
using LegacyApp.Ordering.Api.Models;
using LegacyApp.Ordering.Application;

namespace LegacyApp.Ordering.Api.Controllers
{
    public class CartController : ApiController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [Route("api/cart")]
        public IHttpActionResult CreateCart()
        {
            var id = _cartService.CreateCart();
            return Ok(new CreateCartResponse() {CartId = id});
        }

        [HttpGet]
        [Route("api/cart/{id}")]
        public IHttpActionResult GetCart(int id)
        {
            var cart = _cartService.GetCartById(id);

            if (cart == null)
            {
                return NotFound();
            }

            return Ok(new GetCartResponse()
            {
                CartId = id,
                Total = cart.Items.Sum(i => i.Price * i.Quantity),
                Items = cart.Items.Select(i => new GetCartItemModel()
                {
                    CartItemId = i.CartItemId,
                    ProductId = i.ProductId,
                    Price = i.Price,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity
                }).ToArray()
            });
        }

        [HttpPost]
        [Route("api/cart/{id}/items")]
        public IHttpActionResult CreateItem(int id, [FromBody] CreateItemRequest request)
        {
            _cartService.AddItemToCart(id, request.ProductId);
            return GetCart(id);
        }
    }
}
