using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure.DataAccess.MsSql
{
    public class EFAuthorRepository(AppDbContext Context) : IAuthorRepository
    {
        public IEnumerable<Author> Authors => Context.Authors;

        public async Task<DeleteAuthorResult> DeleteAuthorAsync(int authorId, CancellationToken cancellationToken)
        {
            var author = await Context.Authors.FindAsync([authorId], cancellationToken);
            if (author is null)
                return  new DeleteAuthorResult() { Success = false, Message = "Author not found" };
            
            if(Context.Products.Any(p => p.Author!.Id == authorId))
                return new DeleteAuthorResult() { Success = false, Message = "Author has products" };
            
            Context.Authors.Remove(author);
            await Context.SaveChangesAsync(cancellationToken);
            return new DeleteAuthorResult() { Success = true, Message = "Author deleted successfully" };
        }

        public async Task<Author?> GetAuthorAsync(int authorId, CancellationToken cancellationToken)
        {
            return await Context.Authors.FindAsync([authorId] , cancellationToken);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync(AuthorSearchCriteria criteria, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(criteria, nameof(criteria));
            IQueryable<Author> query = Context.Authors.AsQueryable();
            if (criteria.AuthorId != 0)
                query = query.Where(a => a.Id == criteria.AuthorId);
            if (!string.IsNullOrWhiteSpace(criteria.FirstName))
                query = query.Where(a => a.FirstName.Contains(criteria.FirstName));
            if (!string.IsNullOrWhiteSpace(criteria.LastName))
                query = query.Where(a => a.LastName.Contains(criteria.LastName));
            if (!string.IsNullOrWhiteSpace(criteria.MiddleName))
                query = query.Where(c => c.MiddleName.Contains(criteria.MiddleName));
            
            return await query.ToListAsync(cancellationToken);
        }

        public async Task SaveAuthorAsync(Author author, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(author, nameof(author));
            ArgumentException.ThrowIfNullOrWhiteSpace(author.FirstName, nameof(author.FirstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(author.LastName, nameof(author.LastName));
            ArgumentException.ThrowIfNullOrWhiteSpace(author.MiddleName, nameof(author.MiddleName));
            ArgumentException.ThrowIfNullOrWhiteSpace(author.Info, nameof(author.Info));

            var existingAuthor = await Context.Authors.FindAsync([author.Id], cancellationToken);

            if (existingAuthor == null)
                Context.Authors.Add(author);
            else
            {
                Context.Entry(existingAuthor).CurrentValues.SetValues(author);
            }
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}


