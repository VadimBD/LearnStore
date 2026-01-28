namespace LearnStore.Application.Commands
{
    public record class UpdateAuthorCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
    }
}
