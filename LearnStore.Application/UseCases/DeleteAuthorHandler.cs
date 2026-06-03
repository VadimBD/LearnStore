using LearnStore.Application.Commands.AuthorCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class DeleteAuthorHandler(IAuthorRepository AuthorRepository, IMapper<Author, AuthorDto> Mapper) : IRequestHandler<DeleteAuthorCommand, AuthorDto?>
    {
        public async Task<AuthorDto?> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            if(command.AuthorId == 0)
                throw new ArgumentException("AuthorId cannot be null", nameof(command.AuthorId));
            var result = await AuthorRepository.DeleteAuthorAsync(command.AuthorId, cancellationToken);
            if(!result.Success)
                throw new InvalidOperationException(result.Message);
            var deletedAuthor = await AuthorRepository.GetAuthorAsync(command.AuthorId, cancellationToken);
            return deletedAuthor is not null ? Mapper.ToDto(deletedAuthor) : null;
        }
    }
}
