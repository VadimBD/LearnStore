using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetAuthorHandler(IAuthorRepository AuthorRepository, IMapper<Author, AuthorDto> Mapper) : IRequestHandler<GetAuthorQuery, AuthorDto>
    {
        public async Task<AuthorDto> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            if (request.AuthorId < 1)
                throw new ArgumentOutOfRangeException(nameof(request.AuthorId), request.AuthorId, "AuthorId must be greater than zero.");

            var author = (await AuthorRepository.GetAuthorsAsync(new AuthorSearchCriteria() { AuthorId = request.AuthorId }, cancellationToken)).FirstOrDefault();
            return author is null ? null : Mapper.ToDto(author);
        }
    }
}
