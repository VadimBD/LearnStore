using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public record class GetOrderQuery(Guid OrderId) : IRequest<OrderDto>;
}
