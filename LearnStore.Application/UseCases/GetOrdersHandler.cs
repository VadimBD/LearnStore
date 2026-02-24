using LearnStore.Application.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetOrdersHandler(IOrderRepository OrderRepository, IMapper<Order,OrderDto>  Mapper) : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
    {
        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            var searchCriteria= new OrderSearchCriteria()
            {
                OrderId = query.OrderId,
                CustomerId = query.CustomerId
            };

            return (await OrderRepository.GetOrdersAsync(searchCriteria, cancellationToken)).Select(o=>Mapper.ToDto(o));
        }
    }
}
