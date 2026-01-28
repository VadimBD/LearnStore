using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public class GetOrdersQuery: IRequest<IEnumerable<OrderDto>>
    {
    }
}
