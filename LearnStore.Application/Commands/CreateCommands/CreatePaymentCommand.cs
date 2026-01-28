namespace LearnStore.Application.Commands
{
    public record class CreatePaymentCommand : IRequest<Guid>
    {
        public Guid OrderId { get; init; }
        public decimal Amount { get; init; }
        
    }
}
