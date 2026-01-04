using LearnStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class OrderMapperTests
    {
        private readonly Faker _faker = new();
        private readonly Fixture _fixture = new();


        [Fact]
        public void ToDto_WhenOrderIsNull_ThrowsArgumentNullException()
        {
            var mapper = new OrderMapper();
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("order");

        }
        [Fact]
        public void ToDto_WhenOrderIsValid_ReturnsExpectedDto()
        {
            var mapper = new OrderMapper();
            var orderItemMapper = new OrderItemMapper();
            var customerMapper = new CustomerMapper();
            // Arrange
            Order order = new()
            {
                Id = new(),
                Customer = new() { Id = 1 },
                OrderDate = DateTime.Now,
                Inserted = DateTime.Now,
                Updated = DateTime.Now,
                Items = [new() { Id = 1 }],
                Payments = [new() { Id = new() }],
                State = OrderState.Pending,
            };
            var expectedDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Inserted = order.Inserted,
                Updated = order.Updated,
                Items = [.. order.Items.Select(i => orderItemMapper.ToDto(i))],
                Payments = [.. order.Payments.Select(p => new PaymentMapper().ToDto(p))],
                State = order.State,
                Customer = customerMapper.ToDto(order.Customer),
            };
            // Act
            var result = mapper.ToDto(order);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);
        }
        [Fact]
        public void ToEntity_WhenOrderDtoIsNull_ThrowsArgumentNullException()
        {
            var mapper = new OrderMapper();
            // Act
            Action action = () => mapper.ToEntity(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("orderDto");
        }
        [Fact]
        public void ToEntity_WhenOrderDtoIsValid_ReturnsExpectedEntity()
        {
            var mapper = new OrderMapper();
            var orderItemMapper = new OrderItemMapper();
            var customerMapper = new CustomerMapper();
            // Arrange
            OrderDto orderDto = new()
            {
                Id = new(),
                Customer = new() { Id = 1 },
                OrderDate = DateTime.Now,
                Inserted = DateTime.Now,
                Updated = DateTime.Now,
                Items = [new() { Id = 1 }],
                Payments = [new() { Id = new() }],
                State = OrderState.Pending,
            };
            var expectedEntity = new Order
            {
                Id = orderDto.Id,
                OrderDate = orderDto.OrderDate,
                Inserted = orderDto.Inserted,
                Updated = orderDto.Updated,
                Items = [.. orderDto.Items.Select(i => orderItemMapper.ToEntity(i))],
                Payments = [.. orderDto.Payments.Select(p => new PaymentMapper().ToEntity(p))],
                State = orderDto.State,
                Customer = customerMapper.ToEntity(orderDto.Customer),
            };
            // Act
            var result = mapper.ToEntity(orderDto);
            // Assert
            result.Should().BeEquivalentTo(expectedEntity);
        }
    }
}
