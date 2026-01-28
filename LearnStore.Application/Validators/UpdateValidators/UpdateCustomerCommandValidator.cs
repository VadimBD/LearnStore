using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace LearnStore.Application.Validators
{
    public class UpdateCustomerCommandValidator:AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(c => c!.Id).GreaterThan(0);
            RuleFor(c => c!.Name).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(c => c!.EmailAddress).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress();
            RuleFor(c => c!.PhoneNumber).Cascade(CascadeMode.Stop).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$"); ;
        }
    }
}
