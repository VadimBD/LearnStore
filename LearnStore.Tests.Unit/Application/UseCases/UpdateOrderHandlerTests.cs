
using NSubstitute.ExceptionExtensions;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class UpdateOrderHandlerTests
    {
        private readonly Fixture _fixture = new();
        private List<IMapper> GetMappers()
        {
            var customerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();
            customerMapper.ToDomain(Arg.Any<CustomerDto>())
                .Returns(call =>
                {
                    var dto = call.Arg<CustomerDto>();
                    return new Customer { Id = dto.Id };
                });

            var orderItemMapper = Substitute.For<IMapper<OrderItem, OrderItemDto>>();
            orderItemMapper.ToDomain(Arg.Any<OrderItemDto>())
                .Returns(call =>
                {
                    var dto = call.Arg<OrderItemDto>();
                    return new OrderItem { Id = dto.Id };
                });

            var paymentMapper = Substitute.For<IMapper<Payment, PaymentDto>>();
            paymentMapper.ToDomain(Arg.Any<PaymentDto>())
                .Returns(call =>
                {
                    var dto = call.Arg<PaymentDto>();
                    return new Payment { Id = dto.Id };
                });

            return new List<IMapper> { customerMapper, orderItemMapper, paymentMapper };
        }

        [Fact]
        public async Task Handle_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<UpdateOrderCommand>>();
            validator.ValidateAsync(Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));

            var mappers = GetMappers();

            var handler = new UpdateOrderHandler(orderRepository, validator, mappers);
            Func<Task> act = () => handler.Handle(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("command");
        }

        [Fact]
        public async Task Handle_WhenDeliveryCustomerNull_ThrowsArgumentNullException()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<UpdateOrderCommand>>();
            validator.ValidateAsync(Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));
            var mappers = GetMappers();

            var handler = new UpdateOrderHandler(orderRepository, validator, mappers);
            var command = new UpdateOrderCommand()
            {
                Id = Guid.NewGuid(),
                Items = new List<OrderItemDto>(),
                Customer = null,
                Payments = new List<PaymentDto>()
            };
            Func<Task> act = () => handler.Handle(command, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("Customer");
        }

        [Fact]
        public async Task Handle_WhenCommandIsNotNull_CallsValidationRules()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<UpdateOrderCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));
            var mappers = GetMappers();
            var handler = new UpdateOrderHandler(orderRepository, validator, mappers);
            var command = new UpdateOrderCommand()
            {
                Id = Guid.NewGuid(),
                Items = [new() { Id = 1 }],
                Customer = new() { Id = 1 },
                Payments = [new() { Id = Guid.NewGuid() }]
            };

            await handler.Handle(command, CancellationToken.None);

            await validator.Received(1).ValidateAsync(
                Arg.Is<ValidationContext<UpdateOrderCommand>>(ctx => ctx.InstanceToValidate == command),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_WhenValidatorThrowsException_ThrowsValidationException()
        {
            var orederRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<UpdateOrderCommand>>();

            var validationFailures = new List<ValidationFailure>
            {
                new("Property1", "Error message 1"),
                new("Property2", "Error message 2")
            };

            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>())
                .ThrowsAsync(new FluentValidation.ValidationException(validationFailures));

            var mappers = GetMappers();
            var handler = new UpdateOrderHandler(orederRepository, validator, mappers);

            var command = new UpdateOrderCommand()
            {
                Id = Guid.NewGuid(),
                Items = [new() { Id = 1 }],
                Customer = new() { Id = 1 },
                Payments = [new() { Id = Guid.NewGuid() }]
            };

            Func<Task> act = () => handler.Handle(command, CancellationToken.None);

            var exception = await act.Should().ThrowAsync<FluentValidation.ValidationException>();
            exception.Which.Errors.Should().HaveCount(2);
            exception.Which.Errors.Should().ContainSingle(e => e.PropertyName == "Property1" && e.ErrorMessage == "Error message 1");
            exception.Which.Errors.Should().ContainSingle(e => e.PropertyName == "Property2" && e.ErrorMessage == "Error message 2");
        }
        [Fact]
        public async Task Handle_WhenCommandIsValid_SaveOrder()
        {
            var orderRepository = Substitute.For<IOrderRepository>();
            var validator = Substitute.For<IValidator<UpdateOrderCommand>>();
            validator.ValidateAsync(
                Arg.Any<IValidationContext>(),
                Arg.Any<CancellationToken>()).
                Returns(Task.FromResult(new FluentValidation.Results.ValidationResult()));
            var mappers = GetMappers();
            var handler = new UpdateOrderHandler(orderRepository, validator, mappers);
            var command = new UpdateOrderCommand()
            {
                Id = Guid.NewGuid(),
                Items = [new() { Id = 1 }],
                Customer = new() { Id = 1 },
                Payments = [new() { Id = Guid.NewGuid() }]
            };
            await handler.Handle(command, CancellationToken.None);
            await orderRepository.Received(1).SaveOrderAsync(
                Arg.Is<Order>(o => o.Id == command.Id),
                Arg.Any<CancellationToken>());
        }
    }
}
