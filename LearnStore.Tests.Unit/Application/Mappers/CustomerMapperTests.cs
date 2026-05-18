namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class CustomerMapperTests
    {
      
        private readonly Fixture _fixture = new();

        private IMapperRegistry CreateRegistry()
        {
            return CreateRegistry(null!);
        }

        private IMapperRegistry CreateRegistry(Action<Dictionary<Type, IMapper>> configureMappers)
        {
            var mappers = CreateMappers();
            configureMappers?.Invoke(mappers);
            var registry = Substitute.For<IMapperRegistry>();

            registry.Get<Product, ProductDto>().Returns(mappers[typeof(IMapper<Product, ProductDto>)]);

            return registry;
        }

        private Dictionary<Type, IMapper> CreateMappers()
        {
            var mappers = new Dictionary<Type, IMapper>();
            var prosuctMapper = Substitute.For<IMapper<Product, ProductDto>>();
            prosuctMapper.ToDomain(Arg.Any<ProductDto>()).Returns(new Product());
            prosuctMapper.ToDto(Arg.Any<Product>()).Returns(new ProductDto());
            mappers[typeof(IMapper<Product, ProductDto>)] = prosuctMapper;
            return mappers;
        }
        private IEnumerable<IMapper> GetMappers()
        {
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            return [productMapper];
        }
        [Fact]
        public void ToDto_WhenCustomerIsNull_ThrowsArgumentNullException()
        {
            var registry = CreateRegistry();
            var mapper = new CustomerMapper(registry);
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("customer");
        }
        [Fact]
        public void ToDto_WhenCustomerIsValid_ReturnsExpectedDto()
        {
            var registry = CreateRegistry();
            var mapper = new CustomerMapper(registry);
            var productMapper = registry.Get<Product, ProductDto>();
            // Arrange
            var customer = new Customer()
            {
                Id = 1,
                Name = "John Doe",
                EmailAddress = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                PurchasedProducts = new List<Product>()
            };
            var expectedDto = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,
                PurchasedProducts = customer.PurchasedProducts.Select(p => productMapper.ToDto(p)).ToList()
            };
            // Act
            var result = mapper.ToDto(customer);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);

        }

        [Fact]
       public void ToDomain_WhenCustomerDtoIsNull_ThrowsArgumentNullException()
        {
            var registry = CreateRegistry();
            var mapper = new CustomerMapper(registry);
            // Act
            Action action = () => mapper.ToDomain(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("customerDto");
        }
        [Fact]
        public void ToDomain_WhenCustomerDtoIsValid_ReturnsexpectedDomain()
        {
            var registry = CreateRegistry();
            var mapper = new CustomerMapper(registry);
            var productMapper = registry.Get<Product, ProductDto>();
            // Arrange
            var customerDto = new CustomerDto()
            {
                Id = 1,
                Name = "John Doe",
                EmailAddress = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                PurchasedProducts = new List<ProductDto>()
            }; ;
            var expectedDomain = new Customer
            {
                Id = customerDto.Id,
                Name = customerDto.Name,
                EmailAddress = customerDto.EmailAddress,
                PhoneNumber = customerDto.PhoneNumber,
                PurchasedProducts= customerDto.PurchasedProducts.Select(p => productMapper.ToDomain(p)).ToList()
            };
            // Act
            var result = mapper.ToDomain(customerDto);
            // Assert
            result.Should().BeEquivalentTo(expectedDomain);
        }
    }
}
