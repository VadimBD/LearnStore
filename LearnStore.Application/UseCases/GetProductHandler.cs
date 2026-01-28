using LearnStore.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using LearnStore.Application.Mappers;

namespace LearnStore.Application.UseCases
{
    public class GetProductHandler (IProductRepository ProductRepository,ProductMapper ProductMapper): IRequestHandler<GetProductQuery, ProductDto?>
    {
        public Task<ProductDto?> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            if (query.ProductId < 1)
                throw new ArgumentOutOfRangeException(nameof(query.ProductId), "Product");
           
            var product = ProductRepository.Products.FirstOrDefault(p=>p.Id==query.ProductId);
            return Task.FromResult(product is null ? null:ProductMapper.ToDto(product));
        }
    }
}
