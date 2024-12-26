using ECommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Core;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductCore _productCore;
        public ProductsController(ILogger<ProductsController> logger, ProductCore productCore)
        {
            _logger = logger;
            _productCore = productCore;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<Product> AddProduct([FromBody] ProductDto product)
        {
            _logger.LogInformation("Adding product - Name: {Name}", product.Name);
            _logger.LogDebug("Processing AddProduct request.");

            return await _productCore.AddProduct(product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            _logger.LogInformation("Adding product - Name: {Name}", product.Name);
            _logger.LogDebug("Processing UpdateProduct request.");

            var productUpdate = await _productCore.UpdateProduct(product);
            if (productUpdate == null) return NotFound();

            return Ok("Product updated successfully");
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            _logger.LogInformation("Processing GetProduct request.");
            return await _productCore.GetAllProducts();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            _logger.LogInformation("Get products: {id}", id);
            _logger.LogDebug("Processing GetProduct request.");

            try
            {
                var product = await _productCore.GetProduct(id);
                return product != null ? Ok(product) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting products.");
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProductAsync(int id)
        {
            _logger.LogDebug("Processing RemoveProduct request.");
            var product = await _productCore.DeleteProduct(id);
            if (product == null) return NotFound();

            return Ok("Product deleted successfully");
        }
    }
}
