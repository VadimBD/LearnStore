


namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class CustomerMapperTests
    {
        private readonly Faker _faker = new();
        private readonly Fixture _fixture = new();
        [Fact]
        public void ToDto_WhenCustomerIsNull_ThrowsArgumentNullException()
        {
            var mapper = new CustomerMapper();
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("customer");
        }
        [Fact]
        public void ToDto_WhenCustomerIsValid_ReturnsExpectedDto()
        {
            var mapper = new CustomerMapper();
            // Arrange
            var customer = _fixture.Create<Customer>();
            var expectedDto = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber
            };
            // Act
            var result = mapper.ToDto(customer);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);

        }

        [Fact]
       public void ToEntity_WhenCustomerDtoIsNull_ThrowsArgumentNullException()
        {
            var mapper = new CustomerMapper();
            // Act
            Action action = () => mapper.ToEntity(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("customerDto");
        }
        [Fact]
        public void ToEntity_WhenCustomerDtoIsValid_ReturnsExpectedEntity()
        {
            var mapper = new CustomerMapper();
            // Arrange
            var customerDto = _fixture.Create<CustomerDto>();
            var expectedEntity = new Customer
            {
                Id = customerDto.Id,
                Name = customerDto.Name,
                EmailAddress = customerDto.EmailAddress,
                PhoneNumber = customerDto.PhoneNumber
            };
            // Act
            var result = mapper.ToEntity(customerDto);
            // Assert
            result.Should().BeEquivalentTo(expectedEntity);
        }
    }
}
