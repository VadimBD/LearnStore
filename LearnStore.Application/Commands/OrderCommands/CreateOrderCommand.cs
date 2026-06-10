namespace LearnStore.Application.Commands.OrderCommands
{
    public record class CreateOrderCommand:IRequest<Order>
    {
        public CustomerDto? Customer { get; init; }
        public ICollection<OrderItemDto> Items { get; init; } = [];

    }
}
