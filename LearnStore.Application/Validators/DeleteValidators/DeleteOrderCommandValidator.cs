using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(o => o.OrderId).NotEmpty();
        }
    }
}
