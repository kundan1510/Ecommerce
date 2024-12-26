namespace ECommerce.Shared
{
    class Cart
    {
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}
