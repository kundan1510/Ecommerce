namespace ECommerce.Shared
{
    public class ResponseDto
    {
        public ProductDto? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
