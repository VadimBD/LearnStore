using FluentValidation;
using LearnStore.Application.Commands.OrderCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators.OrderValidators
{
    public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(o => o.Customer)
                .NotNull().ChildRules(customer =>
                {
                    customer.RuleFor(c => c!.Id).NotEmpty();
                });

            RuleFor(o => o.Items).NotEmpty().ForEach(item=>
            {
                item.NotNull().ChildRules(orderItem =>
                {
                    orderItem.RuleFor(i => i!.Product).NotNull().ChildRules(product =>
                    {
                        product.RuleFor(p => p!.Id).NotEmpty();
                    });
                    orderItem.RuleFor(i => i!.Quantity).GreaterThan(0);
                });
            });
               
        }
            

    }
}
