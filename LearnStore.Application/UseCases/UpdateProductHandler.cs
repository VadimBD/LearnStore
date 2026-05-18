using LearnStore.Application.Commands.ProductCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class UpdateProductHandler (IProductRepository ProductRepository,IValidator<UpdateProductCommand> validator,IEnumerable<IMapper> Mappers): IRequestHandler<UpdateProductCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            await validator.ValidateAndThrowAsync(command, cancellationToken);

            var productMapper = Mappers.OfType<IMapper<Product,ProductDto>>().First();
            var authorMapper = Mappers.OfType<IMapper<Author,AuthorDto>>().First();
            var sellerMapper = Mappers.OfType<IMapper<Seller,SellerDto>>().First();
            var categoryMapper = Mappers.OfType<IMapper<ProductCategory,ProductCategoryDto>> ().First();

            var product = new Product()
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                Author = authorMapper.ToDomain(command.Author!),
                Seller = sellerMapper.ToDomain(command.Seller!),
                ChildProducts = command.ChildProducts.Select(cp=>productMapper.ToDomain(cp)).ToList() ,
                Category =categoryMapper.ToDomain(command.Category!),
                IsActive = command.IsActive,
                Price = command.Price
            };
            await ProductRepository.SaveProductAsync(product, cancellationToken);
            return Unit.Value;
        }
    }
}
