using LearnStore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class UpdateOrderHandler(IOrderRepository OrderRepository,IValidator<UpdateOrderCommand> Validator, IEnumerable<IMapper> Mappers) : IRequestHandler<UpdateOrderCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            ArgumentNullException.ThrowIfNull(command.Customer, nameof(command.Customer));
            await Validator.ValidateAndThrowAsync(command, cancellationToken);

            var customerMapper = Mappers.OfType<IMapper<Customer,CustomerDto>>().First();
            var orderItemMapper = Mappers.OfType<IMapper<OrderItem,OrderItemDto>>().First();
            var paymentMapper = Mappers.OfType<IMapper<Payment,PaymentDto>>().First();

            var customer= customerMapper.ToDomain(command.Customer);
            var orderItems = command.Items.Select(item => orderItemMapper.ToDomain(item)).ToList();


            var order = new Order(customer,orderItems) 
            { 
                Id = command.Id,
                Payments = command.Payments.Select(p => paymentMapper.ToDomain(p)).ToList()
            };
            await OrderRepository.SaveOrderAsync(order,cancellationToken);
            return Unit.Value;
        }
    }
}
