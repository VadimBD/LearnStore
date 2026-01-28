using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class UpdateOrderCommandValidator: AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(o => o!.Id).NotEmpty();
            RuleFor(o => o!.Items).NotEmpty();
            RuleForEach(o => o!.Items).NotNull().ChildRules(item =>
            {
                item.RuleFor(i => i!.Id).NotEmpty();
            });
            RuleFor(o => o!.Customer).NotNull().ChildRules(customer =>
            {
                customer.RuleFor(c => c!.Id).NotEmpty();
            });
            RuleFor(o=>o!.Payments).NotNull(); 
            RuleForEach(o => o!.Payments).NotNull().ChildRules(payment =>
            {
                payment.RuleFor(p => p!.Id).NotEmpty();
            });
        }
    }
}
