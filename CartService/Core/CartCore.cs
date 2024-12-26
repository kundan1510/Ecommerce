using CartService.Model;
using CartService.Repositories;

namespace CartService.Core
{
    public class CartCore(IcartRepository cartRepository, ILogger<CartCore> logger)
    {
        private readonly IcartRepository cartRepository = cartRepository;
        private readonly ILogger logger = logger;

        public async Task<IEnumerable<CartItemDto>> GetCartItems()
        {
            logger.LogInformation("Fetching all product details");
            return await cartRepository.GetCartItems();
        }

        public async Task<CartItemDto> AddToCart(CartItemDto item)
        {
            logger.LogInformation("Add item to cart");
            CartItemDto itemResult = await cartRepository.AddToCart(item);
            return itemResult;
        }
    }
}
