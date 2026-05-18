namespace LearnStore.Application.Commands.ProductCategoryCommands
{
    public record class UpdateProductCategoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
