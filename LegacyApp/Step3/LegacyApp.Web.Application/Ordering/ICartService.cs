using System.Threading.Tasks;
using LegacyApp.Ordering.Domain;

namespace LegacyApp.Web.Application.Ordering
{
    public interface ICartService
    {
        Task AddCartItem(int cartId, int productId);

        Task<GetCartQueryResult> GetCartById(int cartId);

        Task<int> CreateCart();
    }
}