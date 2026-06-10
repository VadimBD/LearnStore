using LearnStore.Application.Commands.ProductCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class UpdateProductHandler (IProductRepository ProductRepository,IValidator<UpdateProductCommand> validator,IMapperRegistry mapperRegistry): IRequestHandler<UpdateProductCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            await validator.ValidateAndThrowAsync(command, cancellationToken);

            var productMapper = mapperRegistry.Get<Product, ProductDto>();
            var authorMapper = mapperRegistry.Get<Author, AuthorDto>();
            var sellerMapper = mapperRegistry.Get<Seller, SellerDto>();
            var categoryMapper = mapperRegistry.Get<ProductCategory, ProductCategoryDto>();

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
