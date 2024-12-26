using CartService.Model;

namespace CartService.Repositories
{
    public class CartRepository : IcartRepository
    {
        private List<CartItemDto> _cartItems;

        public CartRepository()
        {
            _cartItems = new List<CartItemDto>()
            {
                new CartItemDto { ProductId = 1, Quantity = 2 },
                new CartItemDto { ProductId = 2, Quantity = 2 },
                new CartItemDto { ProductId = 3, Quantity = 2 }
            };
        }

        public async Task<CartItemDto> AddToCart(CartItemDto item)
        {
            _cartItems.Add(item);
            return item;
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItems()
        {
            return _cartItems;
        }
    }
}
