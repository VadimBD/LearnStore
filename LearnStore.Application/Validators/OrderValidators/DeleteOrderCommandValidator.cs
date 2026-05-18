using FluentValidation;
using LearnStore.Application.Commands.OrderCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.OrderValidators
{
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(o => o.OrderId).NotEmpty();
        }
    }
}
