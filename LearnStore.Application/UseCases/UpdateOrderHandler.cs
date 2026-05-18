using LearnStore.Application.Commands.OrderCommands;
using LearnStore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class UpdateOrderHandler(IOrderRepository OrderRepository,IValidator<UpdateOrderCommand> Validator, IMapperRegistry mapperRegistry) : IRequestHandler<UpdateOrderCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            ArgumentNullException.ThrowIfNull(command.Customer, nameof(command.Customer));
            await Validator.ValidateAndThrowAsync(command, cancellationToken);

            var customerMapper = mapperRegistry.Get<Customer,CustomerDto>() ?? throw new ArgumentException("Customer mapper not found", nameof(mapperRegistry));
            var orderItemMapper = mapperRegistry.Get<OrderItem,OrderItemDto>() ?? throw new ArgumentException("OrderItem mapper not found", nameof(mapperRegistry));
            var paymentMapper = mapperRegistry.Get<Payment,PaymentDto>() ?? throw new ArgumentException("Payment mapper not found", nameof(mapperRegistry));

            var customer= customerMapper.ToDomain(command.Customer);
            var orderItems = command.Items.Select(item => orderItemMapper.ToDomain(item)).ToList();


            var order = new Order(customer,orderItems) 
            { 
                Id = command.Id,
                Payments = command.Payments.Select(p => paymentMapper.ToDomain(p)).ToList(),
                Items = orderItems,
                Updated = DateTime.UtcNow
            };

            await OrderRepository.SaveOrderAsync(order,cancellationToken);
            return Unit.Value;
        }
    }
}
