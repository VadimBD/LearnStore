using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class OrderItemMapperTests
    {
        private readonly Faker _faker = new();
        private readonly Fixture _fixture = new();
        [Fact]
        public void ToDto_WhenOrderItemIsNull_ThrowsArgumentNullException()
        {
            var mapper = new OrderItemMapper();
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("orderItem");
        }
        [Fact]
        public void ToDto_WhenOrderItemIsValid_ReturnsExpectedDto()
        {
            // Arrange
            var productMapper = new ProductMapper();
            var mapper = new OrderItemMapper();
            OrderItem orderItem = new() { 
            Id = _faker.Random.Int(),
            Product = new() {Id=1},
            Quantity = _faker.Random.Int(1, 100),
            PriceAtOrderTime = _faker.Finance.Amount(1, 1000)
            };

            var expectedDto = new OrderItemDto
            {
                Id = orderItem.Id,
                Product = orderItem.Product is null ? null : productMapper.ToDto(orderItem.Product),
                Quantity = orderItem.Quantity,
                PriceAtOrderTime = orderItem.PriceAtOrderTime
            };

            // Act
            var result = mapper.ToDto(orderItem);

            // Assert
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public void ToEntity_WhenOrderItemDtoIsNull_ThrowsArgumentNullException()
        {
            var mapper = new OrderItemMapper();
            // Act
            Action action = () => mapper.ToEntity(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("orderItemDto");
        }
        [Fact]
        public void ToEntity_WhenOrderItemDtoIsValid_ReturnsExpectedEntity()
        {
            var mapper = new OrderItemMapper();
            // Arrange
            OrderItemDto orderItemDto = new()
            {
                Id = _faker.Random.Int(),
                Product = new() { Id = 1 },
                Quantity = _faker.Random.Int(1, 100),
                PriceAtOrderTime = _faker.Finance.Amount(1, 1000)
            };
            var expectedEntity = new OrderItem
            {
                Id = orderItemDto.Id,
                Product = orderItemDto.Product != null ? new Product
                {
                    Id = orderItemDto.Product.Id,
                    Name = orderItemDto.Product.Name,
                    Description = orderItemDto.Product.Description,
                    Price = orderItemDto.Product.Price
                } : null,
                Quantity = orderItemDto.Quantity,
                PriceAtOrderTime = orderItemDto.PriceAtOrderTime
            };
            // Act
            var result = mapper.ToEntity(orderItemDto);
            // Assert
            result.Should().BeEquivalentTo(expectedEntity);
        }
    }
}
