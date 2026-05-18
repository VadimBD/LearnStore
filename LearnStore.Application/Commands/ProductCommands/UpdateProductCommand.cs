namespace LearnStore.Application.Commands.ProductCommands
{
    public record class UpdateProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public AuthorDto? Author { get; set; }
        public SellerDto? Seller { get; set; }
        public ICollection<ProductDto> ChildProducts { get; set; } = [];

        public ProductCategoryDto? Category { get; set; }

        public bool IsActive { get; set; }
        public decimal Price { get; set; }
    }
}
