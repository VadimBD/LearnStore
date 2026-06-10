using LearnStore.Application.Commands.CustomerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class UpdateCustomerHandler(ICustomerRepository CustomerRepository, IMapperRegistry mapperRegistry) : IRequestHandler<UpdateCustomerCommand, Unit>
    {

        public async Task<Unit> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            var productMapper = mapperRegistry.Get<Product, ProductDto>();

            var customer = new Customer()
            {
                Id = command.Id,
                Name = command.Name,
                EmailAddress = command.EmailAddress,
                PhoneNumber = command.PhoneNumber,
                PurchasedProducts = command.Products.Select(p=>productMapper.ToDomain(p)).ToList()
            };

            await CustomerRepository.SaveCustomerAsync(customer, cancellationToken);
            return Unit.Value;
        }
    }
}

