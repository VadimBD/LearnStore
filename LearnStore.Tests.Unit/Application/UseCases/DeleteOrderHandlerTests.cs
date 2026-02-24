using LearnStore.Domain.Entities;

namespace LearnStore.Tests.Unit.Application.UseCases
{
    public class DeleteOrderHandlerTests
    {
        [Fact]
        public async Task Handle_ThrowsArgumentNullException_WhenComandNull()
        {
            // Arrange
            var orderRepository = Substitute.For<IOrderRepository>();
            var mapper = Substitute.For<IMapper<Order, OrderDto>>();
            var handler = new DeleteOrderHandler(orderRepository, mapper);
            // Act 
            Func<Task> act = async () => await handler.Handle(null!, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("query");
        }

        [Fact]
        public async Task Handle_ThrowsArgumentException_WhenOrderIdEmpty()
        {
            // Arrange
            var orderRepository = Substitute.For<IOrderRepository>();
            var mapper = Substitute.For<IMapper<Order, OrderDto>>();
            var handler = new DeleteOrderHandler(orderRepository, mapper);
            var query = new DeleteOrderCommand(Guid.Empty);
            // Act 
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithParameterName("OrderId").WithMessage("OrderId cannot be empty*");
        }

        [Fact]
        public async Task Handle_CallsDeleteOrderAsync_WhenQueryIsValid()
        {
            // Arrange
            var orderRepository = Substitute.For<IOrderRepository>();
            var mapper = Substitute.For<IMapper<Order, OrderDto>>();
            var handler = new DeleteOrderHandler(orderRepository, mapper);

            var orderId = Guid.NewGuid();
            var query = new DeleteOrderCommand(orderId);
            // Act 
            await handler.Handle(query, CancellationToken.None);
            // Assert
            await orderRepository.Received(1).DeleteOrderAsync(orderId, CancellationToken.None);
        }
    }
}
