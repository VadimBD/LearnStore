namespace LearnStore.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> Authors { get; }
        Task<Author?> GetAuthorAsync(int authorId, CancellationToken cancellationToken);
        Task<IEnumerable<Author>> GetAuthorsAsync(AuthorSearchCriteria criteria, CancellationToken cancellationToken);
        
        Task SaveAuthorAsync(Author author, CancellationToken cancellationToken);
        Task<DeleteAuthorResult> DeleteAuthorAsync(int authorId, CancellationToken cancellationToken);
    }
}
