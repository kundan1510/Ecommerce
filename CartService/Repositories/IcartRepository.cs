using CartService.Model;

namespace CartService.Repositories
{
    public interface IcartRepository
    {
        Task<IEnumerable<CartItemDto>> GetCartItems();
        Task<CartItemDto> AddToCart(CartItemDto cartItem);
    }
}
