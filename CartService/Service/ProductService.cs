using CartService.Service.IService;
using ECommerce.Shared;
using Newtonsoft.Json;
using System;

namespace CartService.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("ProductService");
            var response = await client.GetAsync($"api/products");
            var apiContent = await response.Content.ReadAsStringAsync();

            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(apiContent);

            // Return the products
            return products ?? new List<Product>();
        }
    }
}
