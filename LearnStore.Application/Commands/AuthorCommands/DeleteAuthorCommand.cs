namespace LearnStore.Application.Commands.AuthorCommands
{
    public record class DeleteAuthorCommand (int AuthorId) : IRequest<AuthorDto>;
}
