using NSubstitute;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class GetProductHandlerTests
    {
        [Fact]
        public async Task Handle_ThrowsArgumentNullException_WhenQueryIsNull()
        {
            // Arrange
            var productRepository = Substitute.For<IProductRepository>();
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            var handler = new GetProductHandler(productRepository, productMapper);
            // Act
            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("query");
        }
        [Fact]
        public async Task Handle_WhenProductIdLessThanOne_ThrowsArgumentException()
        {

            // Arrange
            var productRepository = Substitute.For<IProductRepository>();
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            var handler = new GetProductHandler(productRepository, productMapper);
            var query = new GetProductQuery(0);
            // Act
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>().WithParameterName("ProductId").WithMessage("ProductId must be greater than zero.*");

        }
        [Fact]
        public async Task Handle_WhenQueryIsValid_ReturnsProduct()
        {
            // Arrange
            var productRepository = Substitute.For<IProductRepository>();
            productRepository.Products.Returns(new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Description = "Test Description", Price = 10.0m },
                new Product { Id = 2, Name = "Product2", Description = "Test Description", Price = 20.0m },
                
            }.AsQueryable());
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            productMapper.ToDto(Arg.Any<Product>()).Returns(new ProductDto() { Id = 1, Name = "Product1", Description = "Test Description", Price = 10.0m });
            var handler = new GetProductHandler(productRepository, productMapper);
            var query = new GetProductQuery(1);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            productMapper.Received(1).ToDto(Arg.Is<Product>(p => p.Id == 1 && p.Name== "Product1" && p.Price== 10.0m));
        }
    }
}
