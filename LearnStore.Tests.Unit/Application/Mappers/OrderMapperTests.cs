using LearnStore.Application.Interfaces;
using LearnStore.Domain.Entities;
using LearnStore.Domain.Enums;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class OrderMapperTests
    {
       

        private IEnumerable<IMapper> CreateMappers()
        {
            var orderItemMapper = Substitute.For<IMapper<OrderItem, OrderItemDto>>();
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();
            var paymentMapper = Substitute.For<IMapper<Payment, PaymentDto>>();
            return [orderItemMapper,customerMapper,paymentMapper];
        }

        [Fact]
        public void ToDto_WhenOrderIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            List<IMapper> mappers = new() 
            {
                Substitute.For<IMapper<OrderItem, OrderItemDto>>(),
                Substitute.For<IMapper<Customer, CustomerDto>>(), 
                Substitute.For<IMapper<Payment, PaymentDto>>() 
            };
            var mapper = new OrderMapper(mappers);

            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("order");

        }
        [Fact]
        public void ToDto_WhenOrderIsValid_ReturnsExpectedDto()
        {
            // Arrange
            Order order = new()
            {
                Id = new(),
                Customer = new() { Id = 1 },
                OrderDate = DateTime.Now,
                Inserted = DateTime.Now,
                Updated = DateTime.Now,
                Items = [new() { Id = 1 }],
                Payments = [new() { Id = Guid.NewGuid() }],
                State = OrderState.Pending,
            };
            var expectedDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Inserted = order.Inserted,
                Updated = order.Updated,
                Items = [new OrderItemDto() {Id=1}],
                Payments = [new PaymentDto() { Id=order.Payments.First().Id}],
                State = order.State,
                Customer =new CustomerDto() {Id=1},
            };

            var mappers = CreateMappers();
            var customerMapper= mappers.OfType<IMapper<Customer, CustomerDto>>().First();
            customerMapper.ToDto(Arg.Any<Customer>()).Returns(expectedDto.Customer);

            var orderItemMapper = mappers.OfType<IMapper<OrderItem, OrderItemDto>>().First();
            orderItemMapper.ToDto(Arg.Any<OrderItem>()).Returns(expectedDto.Items.First());
            var paymentMapper = mappers.OfType<IMapper<Payment, PaymentDto>>().First();
            paymentMapper.ToDto(Arg.Any<Payment>()).Returns(expectedDto.Payments.First());
            var mapper = new OrderMapper(mappers);

            // Act
            var result = mapper.ToDto(order);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);
            customerMapper.Received(1).ToDto(order.Customer);
            orderItemMapper.Received(1).ToDto(Arg.Any<OrderItem>());
            paymentMapper.Received(1).ToDto(Arg.Any<Payment>());
        }
        [Fact]
        public void ToDomain_WhenOrderDtoIsNull_ThrowsArgumentNullException()
        {
            var mappers = CreateMappers();
            var mapper = new OrderMapper(mappers);
            // Act
            Action action = () => mapper.ToDomain(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("orderDto");
        }
        [Fact]
        public void ToDomain_WhenOrderDtoIsValid_ReturnsexpectedDomain()
        {

            var mappers = CreateMappers();
            var mapper = new OrderMapper(mappers);

            // Arrange
            OrderDto orderDto = new()
            {
                Id = new(),
                Customer = new() { Id = 1 },
                OrderDate = DateTime.Now,
                Inserted = DateTime.Now,
                Updated = DateTime.Now,
                Items = [new() { Id = 1 }],
                Payments = [new() { Id = Guid.NewGuid() }],
                State = OrderState.Pending,
            };
            var expectedDomain = new Order
            {
                Id = orderDto.Id,
                OrderDate = orderDto.OrderDate,
                Inserted = orderDto.Inserted,
                Updated = orderDto.Updated,
                Items = [new() { Id = 1 }],
                Payments = [new Payment() { Id = orderDto.Payments.First().Id }],
                State = orderDto.State,
                Customer = new Customer() { Id = 1 },
            };
            // Act
            var customerMapper = mappers.OfType<IMapper<Customer, CustomerDto>>().First();
            customerMapper.ToDomain(Arg.Any<CustomerDto>()).Returns(expectedDomain.Customer);

            var orderItemMapper = mappers.OfType<IMapper<OrderItem, OrderItemDto>>().First();
            orderItemMapper.ToDomain(Arg.Any<OrderItemDto>()).Returns(expectedDomain.Items.First());
            var paymentMapper = mappers.OfType<IMapper<Payment, PaymentDto>>().First();
            paymentMapper.ToDomain(Arg.Any<PaymentDto>()).Returns(expectedDomain.Payments.First());

            var result = mapper.ToDomain(orderDto);
            // Assert
            result.Should().BeEquivalentTo(expectedDomain);
            customerMapper.Received(1).ToDomain(orderDto.Customer);
            orderItemMapper.Received(1).ToDomain(Arg.Any<OrderItemDto>());
            paymentMapper.Received(1).ToDomain(Arg.Any<PaymentDto>());
        }
    }
}
