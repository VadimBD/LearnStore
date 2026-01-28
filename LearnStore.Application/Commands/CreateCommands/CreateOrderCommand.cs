namespace LearnStore.Application.Commands
{
    public record class CreateOrderCommand:IRequest<Guid>
    {
        public CustomerDto? Customer { get; init; }
        public ICollection<OrderItemDto> Items { get; init; } = [];

    }
}
