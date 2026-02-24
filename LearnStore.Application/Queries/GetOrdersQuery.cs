using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public class GetOrdersQuery: IRequest<IEnumerable<OrderDto>>
    {
        public Guid? OrderId { get; init; }
        public Guid? CustomerId { get; init; }
    }
}
