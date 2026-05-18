
using LearnStore.Application.Commands.OrderCommands;
using NSubstitute.ExceptionExtensions;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class UpdateOrderHandlerTests
    {
        private IMapperRegistry CreateRegistry()
        {
            return CreateRegistry(null!);
        }

        private IMapperRegistry CreateRegistry(Action<Dictionary<Type, IMapper>> configureMappers)
        {
            var mappers = CreateMappers();
            configureMappers?.Invoke(mappers);
            var registry = Substitute.For<IMapperRegistry>();

            registry.Get<Customer, CustomerDto>().Returns(mappers[typeof(IMapper<Customer, CustomerDto>)]);
            registry.Get<Payment, PaymentDto>().Returns(mappers[typeof(IMapper<Payment, PaymentDto>)]);

            registry.Get<OrderItem, OrderItemDto>().Returns(mappers[typeof(IMapper<OrderItem, OrderItemDto>)]);

            return registry;
        }

        private Dictionary<Type, IMapper> CreateMappers()
        {
            var mappers = new Dictionary<Type, IMapper>();
            var cuastomerMapper = Substitute.For<IMapper<Customer, CustomerDto>>();
            cuastomerMapper.ToDomain(Arg.Any<CustomerDto>()).Returns(new Customer());
            cuastomerMapper.ToDto(Arg.Any<Customer>()).Returns(new CustomerDto());
            mappers[typeof(IMapper<Customer, CustomerDto>)] = cuastomerMapper;



            var orderItemMapper = Substitute.For<IMapper<OrderItem, OrderItemDto>>();
            orderItemMapper.ToDomain(Arg.Any<OrderItemDto>()).Returns(new OrderItem());
            orderItemMapper.ToDto(Arg.Any<OrderItem>()).Returns(new OrderItemDto());
            mappers[typeof(IMapper<OrderItem, OrderItemDto>)] = orderItemMapper;


            var paymentMapper = Substitute.For<IMapper<Payment, PaymentDto>>();
            paymentMapper.ToDomain(Arg.Any<PaymentDto>()).Returns(new Payment());
            paymentMapper.ToDto(Arg.Any<Payment>()).Returns(new PaymentDto());
            mappers[typeof(IMapper<Payment, PaymentDto>)] = paymentMapper;

            return mappers;
        }
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

            var registry = CreateRegistry();

            var handler = new UpdateOrderHandler(orderRepository, validator, registry);
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
            var registry = CreateRegistry();

            var handler = new UpdateOrderHandler(orderRepository, validator, registry);
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
            var registry = CreateRegistry();
            var handler = new UpdateOrderHandler(orderRepository, validator, registry);
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

            var registry = CreateRegistry();
            var handler = new UpdateOrderHandler(orederRepository, validator, registry);

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
            var registry = CreateRegistry();
            var handler = new UpdateOrderHandler(orderRepository, validator, registry);
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
