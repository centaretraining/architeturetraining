using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LegacyApp.Web.Domain.Ordering;

namespace LegacyApp.Web.Infrastructure.Ordering
{
    public class OrderApiClient : IOrderApiClient
    {
        private static HttpClient _httpClient;

        static OrderApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:31032");
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<CreateCartResponse> CreateCart()
        {
            var response = await _httpClient.PostAsJsonAsync("api/cart", (string)null);
            return await response.Content.ReadAsAsync<CreateCartResponse>();
        }

        public async Task<GetCartResponse> CreateItem(int cartId, int productId)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/cart/{cartId}/items", 
                new CreateItemRequest() { ProductId = productId });
            return await response.Content.ReadAsAsync<GetCartResponse>();
        }

        public async Task<GetCartResponse> GetCartById(int cartId)
        {
            var response = await _httpClient.GetAsync($"api/cart/{cartId}");
            return await response.Content.ReadAsAsync<GetCartResponse>();
        }
    }
}
