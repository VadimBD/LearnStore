using LearnStore.Application.Commands.CustomerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class AddOrderProductsToCustomerHandler(ICustomerRepository CustomerRepository, IOrderRepository OrderRepository) : IRequestHandler<AddOrderProductsToCustomerCommand, Unit>
    {
        public async Task<Unit> Handle(AddOrderProductsToCustomerCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            var order = await OrderRepository.GetOrderAsync(command.OrderId, cancellationToken);
            await CustomerRepository.UpdatePurchasedProductsAsync(order.Customer.Id, order.Items.Select(i=> i.Product).ToList(), cancellationToken );
            return Unit.Value;
        }
    }
}
