using ECommerce.Shared;

namespace CartService.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
