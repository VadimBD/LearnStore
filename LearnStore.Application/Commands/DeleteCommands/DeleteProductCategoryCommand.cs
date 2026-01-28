namespace LearnStore.Application.Commands
{
    public record class DeleteProductCategoryCommand (int ProductId) : IRequest<ProductDto>;
    
}
