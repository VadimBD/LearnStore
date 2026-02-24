using LearnStore.Application.Mappers;
using LearnStore.Domain.Interfaces;
using LearnStore.Domain.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetOrderHandler (IOrderRepository OrderRepository, IMapper<Order,OrderDto> Mapper) : IRequestHandler<GetOrderQuery, OrderDto?>
    {
        public async Task<OrderDto?> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
           ArgumentNullException.ThrowIfNull(query, nameof(query));
            if (query.OrderId == Guid.Empty)
                throw new ArgumentException("OrderId cannot be empty.", nameof(query.OrderId));
            var order = (await OrderRepository.GetOrdersAsync(new OrderSearchCriteria() { OrderId = query.OrderId }, cancellationToken)).FirstOrDefault();
            return order is null ? null : Mapper.ToDto(order);
        }
    }
}
