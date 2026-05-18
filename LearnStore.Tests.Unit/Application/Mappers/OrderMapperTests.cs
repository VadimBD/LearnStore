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

        private IMapperRegistry CreateRegistry()
        {
            return CreateRegistry(null!);
        }

        private IMapperRegistry CreateRegistry(Action<Dictionary<Type, IMapper>> configureMappers)
        {
            var mappers = CreateMappers();
            configureMappers?.Invoke(mappers);
            var registry = Substitute.For<IMapperRegistry>();

            registry.Get<Customer, CustomerDto>().Returns(mappers[typeof(IMapper<Customer, CustomerDto>)]);
            registry.Get<Payment, PaymentDto>().Returns(mappers[typeof(IMapper<Payment, PaymentDto>)]);

            registry.Get<OrderItem, OrderItemDto>().Returns(mappers[typeof(IMapper<OrderItem, OrderItemDto>)]);

            return registry;
        }

        private Dictionary<Type, IMapper> CreateMappers()
        {
            var mappers = new Dictionary<Type, IMapper>();
            var cuastomerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();
            cuastomerMapper.ToDomain(Arg.Any<CustomerDto>()).Returns(new Customer());
            cuastomerMapper.ToDto(Arg.Any<Customer>()).Returns(new CustomerDto());
            mappers[typeof(IMapper<Customer, CustomerDto>)] = cuastomerMapper;

            

            var orderItemMapper = Substitute.For<IMapper<OrderItem, OrderItemDto>>();
            orderItemMapper.ToDomain(Arg.Any<OrderItemDto>()).Returns(new OrderItem());
            orderItemMapper.ToDto(Arg.Any<OrderItem>()).Returns(new OrderItemDto());
            mappers[typeof(IMapper<OrderItem, OrderItemDto>)] = orderItemMapper;


            var paymentMapper = Substitute.For<IMapper<Payment, PaymentDto>>();
            paymentMapper.ToDomain(Arg.Any<PaymentDto>()).Returns(new Payment());
            paymentMapper.ToDto(Arg.Any<Payment>()).Returns(new PaymentDto());
            mappers[typeof(IMapper<Payment, PaymentDto>)] = paymentMapper;

            return mappers;
        }

        [Fact]
        public void ToDto_WhenOrderIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var registry = CreateRegistry();
            var mapper = new OrderMapper(registry);
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
                Customer = new() { Id = "1" },
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
                Customer =new CustomerDto() {Id="1"},
            };

            var registry = CreateRegistry();
            var customerMapper= registry.Get<Customer, CustomerDto>();
            customerMapper.ToDto(Arg.Any<Customer>()).Returns(expectedDto.Customer);

            var orderItemMapper = registry.Get<OrderItem, OrderItemDto>();
            orderItemMapper.ToDto(Arg.Any<OrderItem>()).Returns(expectedDto.Items.First());
            var paymentMapper = registry.Get<Payment, PaymentDto>();
            paymentMapper.ToDto(Arg.Any<Payment>()).Returns(expectedDto.Payments.First());
            var mapper = new OrderMapper(registry);

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
            var registry = CreateRegistry();
            var mapper = new OrderMapper(registry);
            // Act
            Action action = () => mapper.ToDomain(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("orderDto");
        }
        [Fact]
        public void ToDomain_WhenOrderDtoIsValid_ReturnsexpectedDomain()
        {

            var registry = CreateRegistry();
            var mapper = new OrderMapper(registry);

            // Arrange
            OrderDto orderDto = new()
            {
                Id = new(),
                Customer = new() { Id = "1" },
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
                Customer = new Customer() { Id = "1" },
            };
            // Act
            var customerMapper = registry.Get<Customer, CustomerDto>();
            customerMapper.ToDomain(Arg.Any<CustomerDto>()).Returns(expectedDomain.Customer);

            var orderItemMapper = registry.Get<OrderItem, OrderItemDto>();
            orderItemMapper.ToDomain(Arg.Any<OrderItemDto>()).Returns(expectedDomain.Items.First());
            var paymentMapper = registry.Get<Payment, PaymentDto>();
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
