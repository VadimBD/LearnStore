namespace LearnStore.Application.Commands.ProductCategoryCommands
{
    public record class DeleteProductCategoryCommand (int ProductId) : IRequest<ProductDto>;
    
}
