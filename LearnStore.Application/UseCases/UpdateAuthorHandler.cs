using LearnStore.Application.Commands.AuthorCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class UpdateAuthorHandler(IAuthorRepository AuthorRepository, IValidator<UpdateAuthorCommand> Validator) : IRequestHandler<UpdateAuthorCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            ArgumentException.ThrowIfNullOrWhiteSpace(command.FirstName, nameof(command.FirstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(command.LastName, nameof(command.LastName));
            ArgumentException.ThrowIfNullOrWhiteSpace(command.MiddleName, nameof(command.MiddleName));
            ArgumentException.ThrowIfNullOrWhiteSpace(command.Info, nameof(command.Info));

            await Validator.ValidateAndThrowAsync(command, cancellationToken);

            var author = new Author
            {
                Id = command.Id,
                FirstName = command.FirstName,
                LastName = command.LastName,
                MiddleName = command.MiddleName,
                Info = command.Info
            };

            await AuthorRepository.SaveAuthorAsync(author, cancellationToken);

            return Unit.Value;
        }
    }
}
