using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public class GetAuthorQuery : IRequest<AuthorDto>
    {
        public int AuthorId { get; set; }

    }
}
