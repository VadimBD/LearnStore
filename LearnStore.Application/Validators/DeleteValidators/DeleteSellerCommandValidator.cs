using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class DeleteSellerCommandValidator:AbstractValidator<DeleteSellerCommand>
    {
        public DeleteSellerCommandValidator()
        {
            RuleFor(s => s.SellerId).GreaterThan(0);
        }
    }
}
