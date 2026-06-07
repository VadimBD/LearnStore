namespace LearnStore.Application.Commands.PaymentCommands
{
    public record class CreatePaymentCommand : IRequest<Unit>
    {
        public Guid OrderId { get; init; }
        public decimal Amount { get; init; }
        public PaymentStatus Status { get; init; }

        public string TransactionId { get; init; } = string.Empty;

    }
}
