using LegacyApp.Ordering.Api.Models;
using LegacyApp.Web.Application.Ordering;
using LegacyApp.Web.Domain.Ordering;
using Microsoft.AspNetCore.Mvc;

namespace LegacyApp.Ordering.Api.Controllers
{
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IGetCartQuery _getCartQuery;

        public CartController(ICartService cartService, IGetCartQuery getCartQuery)
        {
            _cartService = cartService;
            _getCartQuery = getCartQuery;
        }

        [HttpGet("/{id}")]
        public IActionResult Get(int id)
        {
            var cart = _getCartQuery.GetCartById(id);

            if (cart == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(new GetCartResponse()
            {
                CartId = id,
            });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
