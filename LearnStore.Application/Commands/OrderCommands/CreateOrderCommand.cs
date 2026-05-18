namespace LearnStore.Application.Commands.OrderCommands
{
    public record class CreateOrderCommand:IRequest<Guid>
    {
        public CustomerDto? Customer { get; init; }
        public ICollection<OrderItemDto> Items { get; init; } = [];

    }
}
