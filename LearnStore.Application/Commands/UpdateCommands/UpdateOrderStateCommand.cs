namespace LearnStore.Application.Commands
{
    public record class UpdateOrderStateCommand : IRequest<Unit>
    {
        public Guid OrderId { get; set; } = Guid.Empty;
        public OrderState OrderState { get; set; }
    }
}
