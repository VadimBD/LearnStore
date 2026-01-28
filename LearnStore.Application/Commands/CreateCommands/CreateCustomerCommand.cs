namespace LearnStore.Application.Commands
{
    public record class CreateCustomerCommand : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
