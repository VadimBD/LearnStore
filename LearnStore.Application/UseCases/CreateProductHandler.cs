using LearnStore.Application.Commands.ProductCommands;

namespace LearnStore.Application.UseCases
{
    public class CreateProductHandler(IProductRepository ProductRepository, IValidator<CreateProductCommand> Validator, IMapperRegistry mapperRegistry) : IRequestHandler<CreateProductCommand, Unit>
    {
        public async Task<Unit> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            await Validator.ValidateAndThrowAsync(command, cancellationToken);

            var productMapper = mapperRegistry.Get< Product, ProductDto>();
            var authorMapper = mapperRegistry.Get<Author, AuthorDto>();
            var sellerMapper = mapperRegistry.Get<Seller, SellerDto>();
            var categoryMapper = mapperRegistry.Get<ProductCategory, ProductCategoryDto>();

            var product = new Product()
            {
                Name = command.Name,
                Description = command.Description,
                Author = authorMapper.ToDomain(command.Author!),
                Seller = sellerMapper.ToDomain(command.Seller!),
                ChildProducts = command.ChildProducts.Select(cp => productMapper.ToDomain(cp)).ToList(),
                Category = categoryMapper.ToDomain(command.Category!),
                FileName = command.FileName,
                FileStorageName= command.FileStorageName,
                IsActive = command.IsActive,
                Price = command.Price
            };
            await ProductRepository.SaveProductAsync(product, cancellationToken);
            return Unit.Value;
        }
    }
}
