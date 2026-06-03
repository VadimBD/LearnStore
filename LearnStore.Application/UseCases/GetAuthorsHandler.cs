using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetAuthorsHandler(IAuthorRepository AuthorRepository, IMapper<Author, AuthorDto> Mapper) : IRequestHandler<GetAuthorsQuery, IEnumerable<AuthorDto>>
    {
        public Task<IEnumerable<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var filter = AuthorRepository.Authors.AsQueryable();

            if (request.AuthorId > 0)
                filter = filter.Where(p => p.Id == request.AuthorId);
            if (!string.IsNullOrEmpty(request.FirstName))
                filter = filter.Where(p => p.FirstName.Contains(request.FirstName));
            if (!string.IsNullOrEmpty(request.LastName))
                filter = filter.Where(p => p.LastName.Contains(request.LastName));
            if (!string.IsNullOrEmpty(request.MiddleName))
                filter = filter.Where(p => p.MiddleName.Contains(request.MiddleName));

            var result = filter.Select(p => Mapper.ToDto(p)).ToList();

            return Task.FromResult<IEnumerable<AuthorDto>>(result);
        }
    }
}
