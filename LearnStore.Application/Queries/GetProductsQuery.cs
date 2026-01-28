using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
    }
}
