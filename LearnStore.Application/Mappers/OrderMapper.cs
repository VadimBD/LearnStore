using LearnStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class OrderMapper : IMapper<Order, OrderDto>
    {
        private IMapper<OrderItem,OrderItemDto> _orderItemMapper ;
        private IMapper<Payment,PaymentDto> _paymentMapper;
        private IMapper<Customer,CustomerDto> _customerMapper;

        public OrderMapper(IEnumerable<IMapper> mappers) 
        { 
            _customerMapper= mappers.OfType<IMapper<Customer, CustomerDto>>().FirstOrDefault() ?? throw new ArgumentException("Customer mapper not found", nameof(mappers));
            _orderItemMapper= mappers.OfType<IMapper<OrderItem, OrderItemDto>>().FirstOrDefault() ?? throw new ArgumentException("OrderItem mapper not found", nameof(mappers));
            _paymentMapper= mappers.OfType<IMapper<Payment, PaymentDto>>().FirstOrDefault() ?? throw new ArgumentException("Payment mapper not found", nameof(mappers));
        }

        public Order ToDomain(OrderDto orderDto)
        {

            ArgumentNullException.ThrowIfNull(orderDto, nameof(orderDto));
            return new Order
            {
                Id = orderDto.Id,
                OrderDate = orderDto.OrderDate,
                Inserted = orderDto.Inserted,
                Updated = orderDto.Updated,
                Items = [.. orderDto.Items.Select(i => _orderItemMapper.ToDomain(i))],
                Customer = orderDto.Customer != null ? _customerMapper.ToDomain(orderDto.Customer) : null,
                State = orderDto.State,
                Payments = [.. orderDto.Payments.Select(p => _paymentMapper.ToDomain(p))]
            };
        }

        public object ToDomain(object dto)
        {
            return ToDomain((OrderDto)dto);
        }

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

        public object ToDto(object domain)
        {
            return ToDto((Order)domain);
        }

        
    }
}
