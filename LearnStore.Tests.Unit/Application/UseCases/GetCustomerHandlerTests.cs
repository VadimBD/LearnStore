using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class GetCustomerHandlerTests
    {
        [Fact]
        public async Task Handle_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();
            var handler = new GetCustomerHandler(customerRepository, customerMapper);
            // Act
            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("request");
        }
        [Fact]
        public async Task Handle_WhenCustomerIdEmpty_ThrowsArgumentException()
        {
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();
            var handler = new GetCustomerHandler(customerRepository, customerMapper);
            var query = new GetCustomerQuery(string.Empty);
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>().WithParameterName("Id");
        }
        [Fact]
        public async Task Handle_WhenQueryIsValid_ReturnsCustomer()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();

            var customer = new Customer{Id = "1",Name = "Vadim",EmailAddress = "test@test.com", PhoneNumber = "123456789"};

            var customerDto = new CustomerDto { Id = "1",Name = "Vadim" };

            var query = new GetCustomerQuery("1");

            customerRepository
                .GetCustomerAsync(
                    Arg.Any<CustomerSearchCriteria>(),
                    Arg.Any<CancellationToken>())
                .Returns(customer);

            customerMapper
                .ToDto(customer)
                .Returns(customerDto);

            var handler = new GetCustomerHandler(customerRepository, customerMapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerDto.Id, result.Id);
            Assert.Equal(customerDto.Name, result.Name);

            await customerRepository.Received(1)
                .GetCustomerAsync(
                    Arg.Is<CustomerSearchCriteria>(c => c.Id == "1"),
                    Arg.Any<CancellationToken>());

            customerMapper.Received(1).ToDto(customer);
        }
        [Fact]
        public async Task Handle_WhenCustomerNotFound_ReturnsNull()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();

            customerRepository
                .GetCustomerAsync(
                    Arg.Any<CustomerSearchCriteria>(),
                    Arg.Any<CancellationToken>())
                .Returns((Customer)null!);

            var handler = new GetCustomerHandler(customerRepository, customerMapper);

            var query = new GetCustomerQuery("1");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            customerMapper.DidNotReceive()
                .ToDto(Arg.Any<Customer>());
        }
        [Fact]
        public async Task Handle_WhenCustomerIsNull_DoesNotCallMapper()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();

            customerRepository
                .GetCustomerAsync(
                    Arg.Any<CustomerSearchCriteria>(),
                    Arg.Any<CancellationToken>())
                .Returns((Customer)null!);

            var handler = new GetCustomerHandler(customerRepository, customerMapper);

            // Act
            await handler.Handle(new GetCustomerQuery("1"), CancellationToken.None);

            // Assert
            customerMapper.DidNotReceive()
                .ToDto(Arg.Any<Customer>());
        }
        [Fact]
        public async Task Handle_WhenQueryIsValid_CallsRepositoryOnce()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();

            var customer = new Customer { Id = "1" };

            customerRepository
                .GetCustomerAsync(
                    Arg.Any<CustomerSearchCriteria>(),
                    Arg.Any<CancellationToken>())
                .Returns(customer);

            customerMapper.ToDto(customer).Returns(new CustomerDto());

            var handler = new GetCustomerHandler(customerRepository, customerMapper);

            // Act
            await handler.Handle(new GetCustomerQuery("1"), CancellationToken.None);

            // Assert
            await customerRepository.Received(1)
                .GetCustomerAsync(
                    Arg.Any<CustomerSearchCriteria>(),
                    Arg.Any<CancellationToken>());
        }
        [Fact]
        public async Task Handle_PassesCancellationTokenToRepository()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();

            var token = new CancellationTokenSource().Token;

            customerRepository
                .GetCustomerAsync(
                    Arg.Any<CustomerSearchCriteria>(),
                    token)
                .Returns(new Customer());

            customerMapper
                .ToDto(Arg.Any<Customer>())
                .Returns(new CustomerDto());

            var handler = new GetCustomerHandler(customerRepository, customerMapper);

            // Act
            await handler.Handle(new GetCustomerQuery("1"), token);

            // Assert
            await customerRepository.Received(1)
                .GetCustomerAsync(
                    Arg.Any<CustomerSearchCriteria>(),
                    token);
        }
        
    }
}
