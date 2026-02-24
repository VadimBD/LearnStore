using FluentValidation;
using FluentValidation.Results;
using LearnStore.Domain.Entities;
using LearnStore.Domain.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class CreateOrderHandlerTests
    {
        private readonly Fixture _fixture = new();

        private List<IMapper> GetMappers()
        {
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();
            customerMapper.ToDomain(Arg.Any<CustomerDto>())
                .Returns(new Customer());
            var orderItemMapper = Substitute.For<IMapper<OrderItem, OrderItemDto>>();
            orderItemMapper.ToDomain(Arg.Any<OrderItemDto>())
                .Returns(new OrderItem());
            return [customerMapper, orderItemMapper];
        }

        [Fact]
        public async Task Handle_ThrowsArgumentNullException_WhenComandNull()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<CreateOrderCommand>>();
            validator.ValidateAsync(Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

            var mappers = new List<IMapper>(){
            Substitute.For<IMapper<OrderItem, OrderItemDto>>(),
            Substitute.For<IMapper<Customer,CustomerDto>>()
            };

            var handler = new CreateOrderHandler(orderRepository, validator, mappers);
            Func<Task> act = () => handler.Handle(null!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("command");
        }

        [Fact]
        public async Task Handle_ThrowsArgumentNullException_WhenCustomerNull()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<CreateOrderCommand>>();
            validator.ValidateAsync(Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

            var mappers = new List<IMapper>(){
            Substitute.For<IMapper<OrderItem, OrderItemDto>>(),
            Substitute.For<IMapper<Customer,CustomerDto>>()
            };

            var handler = new CreateOrderHandler(orderRepository, validator, mappers);
            var command = new CreateOrderCommand
            {
                Customer = null!,
                Items = new List<OrderItemDto>()
            };
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("Customer");
        }

        [Fact]
        public async Task Handle_CallsValidationRules_WhenCommandIsNotNull()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<CreateOrderCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));



            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();

            var handler = new CreateOrderHandler(orderRepository, validator, GetMappers());
            var command = new CreateOrderCommand
            {
                Customer = new CustomerDto { Id = 1 },
                Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    Id = 1,
                    Quantity = 1,
                    Product = new ProductDto { Id = 1 }
                }
            }
            };

            // Act
            await handler.Handle(command, CancellationToken.None);

            await validator.Received(1).ValidateAsync(
                Arg.Is<ValidationContext<CreateOrderCommand>>(c => c.InstanceToValidate == command),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ThrowsValidationException_WhenValidatorThrowsException()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<CreateOrderCommand>>();

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Customer", "Customer is required."),
                new ValidationFailure("Items", "At least one order item is required.")
            };

            validator.ValidateAsync(
               Arg.Any<IValidationContext>(),
               Arg.Any<CancellationToken>()).
              ThrowsAsync(new FluentValidation.ValidationException(validationFailures));

            var handler = new CreateOrderHandler(orderRepository, validator, GetMappers());

            var command = new CreateOrderCommand
            {
                Customer = new CustomerDto { Id = 1 },
                Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    Id = 1,
                    Quantity = 1,
                    Product = new ProductDto { Id = 1 }
                }
            }
            };
 
            Func <Task> act = () => handler.Handle(command, CancellationToken.None);

            var exception = await act.Should().ThrowAsync<FluentValidation.ValidationException>();

            exception.Which.Errors.Should().HaveCount(2);
            exception.Which.Errors.Should().Contain(f => f.PropertyName == "Customer" && f.ErrorMessage == "Customer is required.");
            exception.Which.Errors.Should().Contain(f => f.PropertyName == "Items" && f.ErrorMessage == "At least one order item is required.");
        }
        [Fact]
        public  async Task Handle_ReturnsOrderId_WhenCommandIsValid()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var expectedId = Guid.NewGuid();
            orderRepository
                .When(x => x.SaveOrderAsync(Arg.Any<Order>(), CancellationToken.None))
                .Do(call =>
                {
                    var order = call.Arg<Order>();
                    order.Id = expectedId;
                });

            var validator = Substitute.For<IValidator<CreateOrderCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));
            
            var handler = new CreateOrderHandler(orderRepository, validator, GetMappers());

            var command = new CreateOrderCommand
            {
                Customer = new CustomerDto { Id = 1 },
                Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    Id = 1,
                    Quantity = 1,
                    Product = new ProductDto { Id = 1 }
                }
            }
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBe(Guid.Empty);

            await orderRepository.Received(1).SaveOrderAsync(
                Arg.Is<Order>(o => o.Id == result),
                Arg.Any<CancellationToken>());
        }
        
        [Fact]
        public async Task Handle_SaveOrder_WhenCommandIsValid()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<CreateOrderCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

            var handler = new CreateOrderHandler(orderRepository, validator, GetMappers());

            var command = new CreateOrderCommand
            {
                Customer = new CustomerDto { Id = 1 },
                Items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    Id = 1,
                    Quantity = 1,
                    Product = new ProductDto { Id = 1 }
                }
            }
            };

            await handler.Handle(command, CancellationToken.None);

            await orderRepository.Received(1).SaveOrderAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
        }

    }
}