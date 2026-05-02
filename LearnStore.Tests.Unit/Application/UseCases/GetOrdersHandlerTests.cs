namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class GetOrdersHandlerTests
    {
        [Fact]
        public async Task Handle_WhenComandNull_ThrowsArgumentNullException()
        {
            // Arrange
            var orderRepository = Substitute.For<IOrderRepository>();
            var mapper = Substitute.For<IMapper<Order, OrderDto>>();
            var handler = new GetOrdersHandler(orderRepository, mapper);
            // Act 
            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("query");
        }
        [Fact]
        public async Task Handle_WhenQueryIsValid_CallsGetOrdersAsync()
        {
            // Arrange
            var orderRepository = Substitute.For<IOrderRepository>();
            var mapper = Substitute.For<IMapper<Order, OrderDto>>();
            var handler = new GetOrdersHandler(orderRepository, mapper);
            var orderId = Guid.NewGuid();
            var customerId = 1;
            var query = new GetOrdersQuery
            {
                OrderId = orderId,
                CustomerId = customerId
            };
            // Act 
            await handler.Handle(query, CancellationToken.None);
            // Assert
            await orderRepository.Received(1).GetOrdersAsync(Arg.Is<OrderSearchCriteria>(c => c.OrderId == orderId && c.CustomerId == customerId), Arg.Any<CancellationToken>());
        }
    }
}
