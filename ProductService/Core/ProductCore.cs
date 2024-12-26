using ECommerce.Shared;
using ProductService.Repositories;

namespace ProductService.Core
{
    public class ProductCore(IProductRepository productRepository, ILogger<ProductCore> logger)
    {
        private readonly IProductRepository productRepository = productRepository;
        private readonly ILogger logger = logger;



        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            logger.LogInformation("Fetching all product details");
            return await productRepository.GetAllProducts();
        }

        public async Task<Product> AddProduct(ProductDto product)
        {
            logger.LogInformation("Add product");
            Product productResult = await productRepository.Add(product);
            return productResult;
        }
        public async Task<Product> GetProduct(int productId)
        {
            logger.LogInformation("Fetching product details");
            Product productResult = await productRepository.GetProduct(productId);
            return productResult;
        }
        public async Task<Product> DeleteProduct(int productId)
        {
            logger.LogInformation("Deleting product with id: {productId}", productId);
            Product productResult = await productRepository.Delete(productId);
            return productResult;
        }
        public async Task<Product> UpdateProduct(Product product)
        {
            logger.LogInformation("Updating product with id: {productId}", product.Id);
            Product productResult = await productRepository.Update(product);
            return productResult;
        }
    }
}
