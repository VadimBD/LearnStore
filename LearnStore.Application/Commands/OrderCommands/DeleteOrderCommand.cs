namespace LearnStore.Application.Commands.OrderCommands
{
    public record class DeleteOrderCommand (Guid OrderId): IRequest<OrderDto?>;
}
