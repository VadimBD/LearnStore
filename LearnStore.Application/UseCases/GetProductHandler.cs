using LearnStore.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using LearnStore.Application.Mappers;

namespace LearnStore.Application.UseCases
{
    public class GetProductHandler (IProductRepository ProductRepository,IMapper<Product,ProductDto> ProductMapper): IRequestHandler<GetProductQuery, ProductDto?>
    {
        public async Task<ProductDto?> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            if (query.ProductId < 1)
               throw new ArgumentOutOfRangeException(nameof(query.ProductId),query.ProductId,"ProductId must be greater than zero."); 
           
            var product = await ProductRepository.GetProductAsync(query.ProductId,cancellationToken);
            return product is null ? null:ProductMapper.ToDto(product);
        }
    }
}
