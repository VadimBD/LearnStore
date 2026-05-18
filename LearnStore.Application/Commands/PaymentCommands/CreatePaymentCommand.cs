namespace LearnStore.Application.Commands.PaymentCommands
{
    public record class CreatePaymentCommand : IRequest<Guid>
    {
        public Guid OrderId { get; init; }
        public decimal Amount { get; init; }
        
    }
}
