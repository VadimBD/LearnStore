using LearnStore.Application.Commands.CustomerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class CreateCustomerHandler(ICustomerRepository CustomerRepository, IValidator<CreateCustomerCommand> Validator) : IRequestHandler<CreateCustomerCommand, Unit>
    {
        public async Task<Unit> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            ArgumentNullException.ThrowIfNull(command.Name, nameof(command.Name));
            ArgumentNullException.ThrowIfNull(command.EmailAddress, nameof(command.EmailAddress));
            ArgumentNullException.ThrowIfNull(command.PhoneNumber, nameof(command.PhoneNumber));

            await Validator.ValidateAndThrowAsync(command, cancellationToken);
            var customers = await CustomerRepository.GetCustomersAsync(new CustomerSearchCriteria { EmailAddress = command.EmailAddress }, cancellationToken);
            if(!customers.Any())
            {
                Customer customer = new Customer()
                {
                    Name = command.Name,
                    EmailAddress = command.EmailAddress,
                    PhoneNumber = command.PhoneNumber
                };
                await CustomerRepository.SaveCustomerAsync(customer, cancellationToken);
                return Unit.Value;
            }
            throw new InvalidOperationException("CustomerEmailAlreadyExists");
        }
    }
}
