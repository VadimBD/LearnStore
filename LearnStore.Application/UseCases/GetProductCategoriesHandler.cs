using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetProductCategoriesHandler (IProductRepository productRepository): IRequestHandler<GetProductCategoriesQuery, IEnumerable<ProductCategoryDto>>
    {
        public Task<IEnumerable<ProductCategoryDto>> Handle(GetProductCategoriesQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query,nameof(query));
            return Task.FromResult(productRepository.Categories.Select(c => new ProductCategoryDto() { Id = c.Id, Name = c.Name }));
       }
    }
}
