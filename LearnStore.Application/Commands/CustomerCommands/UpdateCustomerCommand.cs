namespace LearnStore.Application.Commands.CustomerCommands
{
    public record class UpdateCustomerCommand : IRequest<Unit>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public IEnumerable<ProductDto> Products { get; set; } = [];
    }
}
