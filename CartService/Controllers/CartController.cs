using CartService.Kafka;
using CartService.Model;
using CartService.Service.IService;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private IProductService _productService;
        private IKafkaProducer _producer;
        private readonly ILogger<CartController> _logger;
        private readonly Core.CartCore _cartCore;
        public CartController(IProductService productService, IKafkaProducer producer, ILogger<CartController> logger, Core.CartCore cartCore)
        {
            _productService = productService;
            _producer = producer;
            _logger = logger;
            _cartCore = cartCore;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto item)
        {
            _logger.LogInformation("Adding item to cart: {ProductId}, Quantity: {Quantity}", item.ProductId, item.Quantity);
            _logger.LogDebug("Processing AddToCart request.");

            try
            {
                var cart = await _cartCore.AddToCart(item);
                _logger.LogInformation("Item successfully added to cart.");
                return cart != null ? Ok(cart) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding item to cart.");
                return StatusCode(500, "An error occurred.");
            }
        }

        [HttpGet]
        public async Task<IEnumerable<CartItemDto>> GetCartItems()
        {
            _logger.LogInformation("Processing GetCartItems request.");
            return await _cartCore.GetCartItems();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var cartItems = await _cartCore.GetCartItems();
            _logger.LogInformation("Processing Checkout request.");
            IEnumerable<ECommerce.Shared.Product> productDtos = await _productService.GetProducts();

            decimal totalPrice = 0;
            try
            {
                foreach (var item in cartItems)
                {
                    var product = productDtos.FirstOrDefault(u => u.Id == item.ProductId);
                    totalPrice += item.Quantity * (product?.Price ?? 0);
                }
                //cartItems.Clear();


                //Create a order message
                var message = new ECommerce.Shared.CheckoutMessage
                {
                    Id = 1,
                    Message = "Checkout producer message "
                };

                await _producer.ProduceAsync("order-topic", new Message<string, string>
                {
                    Key = message.Id.ToString(),
                    Value = JsonSerializer.Serialize(message)
                });

                _logger.LogInformation("Item successfully added to cart.");
                return Ok($"Checkout Successful!---> Total Price: {totalPrice}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Checkout to cart.");
                return StatusCode(500, "An error occurred.");
            }
        }

    }
}
