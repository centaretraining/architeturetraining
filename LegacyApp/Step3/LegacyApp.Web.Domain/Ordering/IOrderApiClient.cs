using System.Threading.Tasks;

namespace LegacyApp.Web.Domain.Ordering
{
    public interface IOrderApiClient
    {
        Task<CreateCartResponse> CreateCart();

        Task<GetCartResponse> CreateItem(int cartId, int productId);

        Task<GetCartResponse> GetCartById(int cartId);
    }
}
