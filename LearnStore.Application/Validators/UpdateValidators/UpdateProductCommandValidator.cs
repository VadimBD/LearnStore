using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Validators
{
    public class UpdateProductCommandValidator:AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(p => p!.Id).GreaterThan(0);
            RuleFor(p => p!.Name).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(p => p!.Description).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(p => p!.Price).GreaterThan(0);
            RuleForEach(p => p!.ChildProducts).NotNull().ChildRules(childProduct =>{childProduct.RuleFor(cp => cp!.Id).GreaterThan(0);});
            RuleFor(p => p!.Author).NotNull().ChildRules(author =>{ author.RuleFor(a => a!.Id).GreaterThan(0);});
            RuleFor(p => p!.Seller).NotNull().ChildRules(seller =>{ seller.RuleFor(s => s!.Id).GreaterThan(0);});
            RuleFor(p => p!.Category).NotNull().ChildRules(category =>{ category.RuleFor(c => c!.Id).GreaterThan(0);});
            RuleFor(p => p!.IsActive).NotNull();
        }
    }
}
