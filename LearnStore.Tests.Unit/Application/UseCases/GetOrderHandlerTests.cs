namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class GetOrderHandlerTests
    {
        [Fact]
        public async Task Handle_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var orderRepository = Substitute.For<IOrderRepository>();

            var mapper = Substitute.For<IMapper<Order, OrderDto>>();

            var handler = new GetOrderHandler(orderRepository, mapper);
            // Act 
            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("query");
        }

        [Fact]
        public async Task Handle_WhenOrderIdEmpty_ThrowsArgumentException()
        {
            // Arrange
            var orderRepository = Substitute.For<IOrderRepository>();
            var mapper = Substitute.For<IMapper<Order, OrderDto>>();
            var handler = new GetOrderHandler(orderRepository, mapper);
            var query = new GetOrderQuery(Guid.Empty);

            // Act 
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("OrderId cannot be empty.*");
        }

        [Fact]
        public async Task Handle_WhenQueryIsValid_CallsGetOrdersAsync()
        {

            // Arrange
            var orderRepository = Substitute.For<IOrderRepository>();
            var mapper = Substitute.For<IMapper<Order, OrderDto>>();
            var handler = new GetOrderHandler(orderRepository, mapper);
            var orderId = Guid.NewGuid();

            var query = new GetOrderQuery(orderId);

            // Act 
            await handler.Handle(query, CancellationToken.None);
            // Assert
            await orderRepository.Received(1).GetOrdersAsync(Arg.Is<OrderSearchCriteria>(c => c.OrderId == orderId), Arg.Any<CancellationToken>());

        }

    }
}
