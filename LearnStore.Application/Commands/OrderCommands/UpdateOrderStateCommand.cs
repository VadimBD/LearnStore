namespace LearnStore.Application.Commands.OrderCommands
{
    public record class UpdateOrderStateCommand : IRequest<Unit>
    {
        public Guid OrderId { get; set; } = Guid.Empty;
        public OrderState OrderState { get; set; }
    }
}
