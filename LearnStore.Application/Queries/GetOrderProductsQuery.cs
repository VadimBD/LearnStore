using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public record class GetOrderProductsQuery: IRequest<IEnumerable<ProductDto>>
    {
        public Guid Orderid { get; set; }
    }
}
