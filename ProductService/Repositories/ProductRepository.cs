using ECommerce.Shared;

namespace ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _productList;

        public ProductRepository()
        {
            _productList = new List<Product>()
        {
            new Product { Id = 1, Name = "Laptop", Description = "A high-performance laptop", Price = 1500.00M, Size = "15 inch" },
            new Product { Id = 2, Name = "Smartphone", Description = "A flagship smartphone", Price = 800.00M, Size = "6 inch" },
            new Product { Id = 3, Name = "Headphones", Description = "Noise-cancelling headphones", Price = 200.00M, Size = "One Size" }
        };
        }

        public async Task<Product> Add(ProductDto product)
        {
            int id = _productList.Max(e => e.Id) + 1;
            Product productAdded = new Product { Id = id, Name = product.Name, Description = product.Description, Price = product.Price, Size = product.Size };

            _productList.Add(productAdded);
            return productAdded;
        }

        public async Task<Product> Delete(int Id)
        {
            Product product = _productList.FirstOrDefault(e => e.Id == Id);
            if (product != null)
            {
                _productList.Remove(product);
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return _productList;
        }

        public async Task<Product> GetProduct(int Id)
        {
            return this._productList.FirstOrDefault(e => e.Id == Id);
        }

        public async Task<Product> Update(Product productChanges)
        {
            Product product = _productList.FirstOrDefault(e => e.Id == productChanges.Id);
            if (product != null)
            {
                product.Name = productChanges.Name;
                product.Description = productChanges.Description;
                product.Price = productChanges.Price;
                product.Size = productChanges.Size;
            }
            return product;
        }
    }
}
