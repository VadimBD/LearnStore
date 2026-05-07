namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class CustomerMapperTests
    {
      
        private readonly Fixture _fixture = new();

        private IEnumerable<IMapper> GetMappers()
        {
            var productMapper = Substitute.For<IMapper<Product, ProductDto>>();
            return [productMapper];
        }
        [Fact]
        public void ToDto_WhenCustomerIsNull_ThrowsArgumentNullException()
        {
            var mapper = new CustomerMapper(GetMappers());
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("customer");
        }
        [Fact]
        public void ToDto_WhenCustomerIsValid_ReturnsExpectedDto()
        {
            var mapper = new CustomerMapper(GetMappers());
            var productMapper = GetMappers().OfType<IMapper<Product, ProductDto>>().First();
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
            var mapper = new CustomerMapper(GetMappers());
            // Act
            Action action = () => mapper.ToDomain(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("customerDto");
        }
        [Fact]
        public void ToDomain_WhenCustomerDtoIsValid_ReturnsexpectedDomain()
        {
            var mapper = new CustomerMapper(GetMappers());
            var productMapper = GetMappers().OfType<IMapper<Product, ProductDto>>().First();
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
