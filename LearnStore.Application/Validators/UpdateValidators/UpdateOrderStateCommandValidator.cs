using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class UpdateOrderStateCommandValidator : AbstractValidator<UpdateOrderStateCommand>
    {
        public UpdateOrderStateCommandValidator()
        {
            RuleFor(o => o!.OrderId).NotEmpty();

            RuleFor(o => o!.OrderState).IsInEnum();
        }
    }
}
