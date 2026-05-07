namespace LearnStore.Application.Commands
{
    public record class DeleteOrderCommand (Guid OrderId): IRequest<OrderDto?>;
}
