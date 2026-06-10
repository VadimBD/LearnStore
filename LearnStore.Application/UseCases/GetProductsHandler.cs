
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetProductsHandler (IProductRepository ProductRepository, IMapper<Product,ProductDto> Mapper) : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            var criteria = new ProductSearchCriteria
            {
                Name = query.ProductName,
                Author=new Author { Id = query.AuthorId },
                Seller = new Seller { Id = query.SellerId },
                Category = new ProductCategory { Id = query.CategoryId },
                Price = query.Price,
                IsActive=query.IsActive,
            };  
            
            var products = await ProductRepository.GetProductsAsync(criteria, cancellationToken);
            return products.Select(p => Mapper.ToDto(p));
        }
    }
}
