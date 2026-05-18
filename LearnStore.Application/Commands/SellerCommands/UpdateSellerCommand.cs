namespace LearnStore.Application.Commands.SellerCommands
{
    public record class UpdateSellerCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public decimal AccountBalance { get; set; } = decimal.Zero;
    }
}
