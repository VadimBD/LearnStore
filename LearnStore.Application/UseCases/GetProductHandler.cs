using LearnStore.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace LearnStore.Application.UseCases
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, ProductDto?>
    {
        public Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
