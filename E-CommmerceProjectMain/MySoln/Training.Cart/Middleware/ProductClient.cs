using DataLayer.Entities;
using System.Net.Http;

namespace Training.Cart.Middleware
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "http://localhost:7186/api/Product/";

        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves product information from an external API based on the specified product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// Returns the product entity if found; otherwise, null.</returns>}
        public async Task<ProductEntity> GetProduct(int productId)
        {
            var productEndpointUrl = $"{baseUrl}findproduct/{productId}"; 
            var prod1 = await _httpClient.GetFromJsonAsync<ProductEntity>(productEndpointUrl);
            return prod1;
        }
    }
}
