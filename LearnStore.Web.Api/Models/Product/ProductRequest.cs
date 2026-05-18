namespace LearnStore.Web.Api.Models.Product
{
    public record class ProductRequest
    {
        public int SellerId { get; init; }
        public int CategoryId { get; init; }
        public int AuthorId { get; init; }
        public bool IsActive { get; init; }
        public decimal Price { get; init; }

        public string ProductName { get; init; }
    }
}
