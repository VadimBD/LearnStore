using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class OrderMapper
    {
        private OrderItemMapper _orderItemMapper = new();
        private PaymentMapper _paymentMapper = new();
        private CustomerMapper _customerMapper = new();

        public OrderDto ToDto (Order order)
        {
            ArgumentNullException.ThrowIfNull(order,nameof(order));
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Inserted = order.Inserted,
                Updated = order.Updated,
                Items = [..order.Items.Select(i => _orderItemMapper.ToDto(i))],
                Customer = order.Customer != null ? _customerMapper.ToDto(order.Customer) : null,
                State = order.State,
                Payments = [..order.Payments.Select(p => _paymentMapper.ToDto(p))]
            };
        }
        public Order ToEntity (OrderDto orderDto)
        {
            ArgumentNullException.ThrowIfNull(orderDto,nameof(orderDto));
           return new Order
            {
                Id = orderDto.Id,
                OrderDate = orderDto.OrderDate,
                Inserted = orderDto.Inserted,
                Updated = orderDto.Updated,
                Items = [..orderDto.Items.Select(i => _orderItemMapper.ToEntity(i))],
                Customer = orderDto.Customer != null ? _customerMapper.ToEntity(orderDto.Customer) : null,
                State = orderDto.State,
                Payments = [..orderDto.Payments.Select(p => _paymentMapper.ToEntity(p))]
            };
        }
    }
}
