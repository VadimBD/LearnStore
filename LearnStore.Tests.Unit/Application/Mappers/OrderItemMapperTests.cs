
namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class OrderItemMapperTests
    {
        private readonly Faker _faker = new();
       
        [Fact]
        public void ToDto_WhenOrderItemIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            var mapper = new OrderItemMapper(productMapper);
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("orderItem");
        }
        [Fact]
        public void ToDto_WhenOrderItemIsValid_ReturnsExpectedDto()
        {
            // Arrange
            // Arrange
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            var mapper = new OrderItemMapper(productMapper);
            OrderItem orderItem = new() { 
            Id = 1,
            Product = new() {Id=1},
            Quantity = 10,
            PriceAtOrderTime = 10m
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
        public void ToDomain_WhenOrderItemDtoIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            var mapper = new OrderItemMapper(productMapper);
            // Act
            Action action = () => mapper.ToDomain(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("orderItemDto");
        }
        [Fact]
        public void ToDomain_WhenOrderItemDtoIsValid_ReturnsexpectedDomain()
        {
           
           
            // Arrange
            OrderItemDto orderItemDto = new()
            {
                Id = 1,
                Product = new() { Id = 1 },
                Quantity = 10,
                PriceAtOrderTime = 10m
            };
            var expectedDomain = new OrderItem
            {
                Id = orderItemDto.Id,
                Product = new() { Id=1},
                Quantity = orderItemDto.Quantity,
                PriceAtOrderTime = orderItemDto.PriceAtOrderTime
            };
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            productMapper.ToDomain(Arg.Any<ProductDto>()).Returns(expectedDomain.Product);
                
            var mapper = new OrderItemMapper(productMapper);

            // Act
            var result = mapper.ToDomain(orderItemDto);
            // Assert
            result.Should().BeEquivalentTo(expectedDomain);
        }
    }
}
