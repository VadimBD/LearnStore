using LearnStore.Application.Commands.SellerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.SellerValidators
{
    public class DeleteSellerCommandValidator:AbstractValidator<DeleteSellerCommand>
    {
        public DeleteSellerCommandValidator()
        {
            RuleFor(s => s.SellerId).GreaterThan(0);
        }
    }
}
