using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class DeleteOrderHandler (IOrderRepository OrderRepository,IMapper<Order,OrderDto> Mapper): IRequestHandler<DeleteOrderCommand, OrderDto>
    {
        public async Task<OrderDto> Handle(DeleteOrderCommand query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            if(query.OrderId == Guid.Empty)
                throw new ArgumentException("OrderId cannot be empty", nameof(query.OrderId));

            var order= await OrderRepository.DeleteOrderAsync(query.OrderId, cancellationToken);
            return order!=null ? Mapper.ToDto(order) : null;
        }
    }
}
