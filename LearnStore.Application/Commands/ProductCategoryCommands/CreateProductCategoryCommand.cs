namespace LearnStore.Application.Commands.ProductCategoryCommands
{
    public record class CreateProductCategoryCommand : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
    }
}
