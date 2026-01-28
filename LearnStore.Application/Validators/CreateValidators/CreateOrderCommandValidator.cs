using FluentValidation;

using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(o => o.Customer)
                .NotNull().ChildRules(customer =>
                {
                    customer.RuleFor(c => c!.Id).GreaterThan(0);
                });

            RuleFor(o => o.Items).NotEmpty().ForEach(item=>
            {
                item.NotNull().ChildRules(orderItem =>
                {
                    orderItem.RuleFor(i => i!.Id).GreaterThan(0);
                    orderItem.RuleFor(i => i!.Product).NotNull().ChildRules(product =>
                    {
                        product.RuleFor(p => p!.Id).GreaterThan(0);
                    });
                    orderItem.RuleFor(i => i!.Quantity).GreaterThan(0);
                });
            });
               
        }
            

    }
}
