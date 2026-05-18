using LearnStore.Application.Commands.AuthorCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.AuthorValidators
{
    public class DeleteAuthorCommandValidator:AbstractValidator<DeleteAuthorCommand>
    {
        public DeleteAuthorCommandValidator()
        {
            RuleFor(a => a.AuthorId).GreaterThan(0);
        }
    }
}
