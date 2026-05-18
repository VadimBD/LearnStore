namespace LearnStore.Application.Commands.AuthorCommands
{
    public record class CreateAuthorCommand : IRequest<Unit>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
    }
}
