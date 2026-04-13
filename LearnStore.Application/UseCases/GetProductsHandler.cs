
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetProductsHandler (IProductRepository ProductRepository, IMapper<Product,ProductDto> Mapper) : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        public Task<IEnumerable<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            var filter = ProductRepository.Products.AsQueryable();

            if (query.SellerId > 0)
                filter = filter.Where(p => p.Seller != null && p.Seller.Id == query.SellerId);
            if (!string.IsNullOrEmpty(query.ProductName))
                filter = filter.Where(p => p.Name.Contains(query.ProductName));
            if (query.CategoryId > 0)
                filter = filter.Where(p => p.Category != null && p.Category.Id == query.CategoryId);
            if (query.AuthorId > 0)
                filter = filter.Where(p => p.Author != null && p.Author.Id == query.AuthorId);
            if (query.IsActive)
                filter = filter.Where(p => p.IsActive);
            if (query.Price > 0)
                filter = filter.Where(p => p.Price <= query.Price);

            var result = filter.Select(p=>Mapper.ToDto(p)).ToList();

            return Task.FromResult<IEnumerable<ProductDto>>(result);
        }
    }
}
