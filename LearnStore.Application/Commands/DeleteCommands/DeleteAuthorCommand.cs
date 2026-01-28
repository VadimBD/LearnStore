namespace LearnStore.Application.Commands
{
    public record class DeleteAuthorCommand (int AuthorId) : IRequest<AuthorDto>;
}
