using ECommerce.Shared;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(int Id);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> Add(ProductDto product);
        Task<Product> Update(Product productChanges);
        Task<Product> Delete(int Id);
    }
}
