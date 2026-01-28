using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class DeleteAuthorCommandValidator:AbstractValidator<DeleteAuthorCommand>
    {
        public DeleteAuthorCommandValidator()
        {
            RuleFor(a => a.AuthorId).GreaterThan(0);
        }
    }
}
