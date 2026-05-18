using LearnStore.Application.Commands.ProductCommands;

namespace LearnStore.Application.UseCases
{
    public class CreateProductHandler(IProductRepository ProductRepository, IValidator<CreateProductCommand> Validator, IEnumerable<IMapper> Mappes) : IRequestHandler<CreateProductCommand, Unit>
    {
        public async Task<Unit> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            await Validator.ValidateAndThrowAsync(command, cancellationToken);

            var productMapper = Mappes.OfType< IMapper<Product,ProductDto>>().First();
            var authorMapper = Mappes.OfType<IMapper<Author,AuthorDto>>().First();
            var sellerMapper = Mappes.OfType<IMapper<Seller,SellerDto>>().First();
            var categoryMapper = Mappes.OfType<IMapper<ProductCategory,ProductCategoryDto>>().First();

            var product = new Product()
            {
                Name = command.Name,
                Description = command.Description,
                Author = authorMapper.ToDomain(command.Author!),
                Seller = sellerMapper.ToDomain(command.Seller!),
                ChildProducts = command.ChildProducts.Select(cp => productMapper.ToDomain(cp)).ToList(),
                Category = categoryMapper.ToDomain(command.Category!),
                IsActive = command.IsActive,
                Price = command.Price
            };
            await ProductRepository.SaveProductAsync(product, cancellationToken);
            return Unit.Value;
        }
    }
}
