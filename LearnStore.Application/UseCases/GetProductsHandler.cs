
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        public Task<IEnumerable<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
