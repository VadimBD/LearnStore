using LearnStore.Application.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class CreateOrderHandler( IOrderRepository OrderRepository, IValidator<CreateOrderCommand> ValidationRules,IEnumerable<IMapper> Mappers) : IRequestHandler<CreateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            ArgumentNullException.ThrowIfNull(command.Items, nameof(command.Items));
            ArgumentNullException.ThrowIfNull(command.Customer, nameof(command.Customer));
            await ValidationRules.ValidateAndThrowAsync(command, cancellationToken);

            var customerMapper = Mappers.OfType<IMapper<Customer, CustomerDto>>().First();
            var orderItemMapper = Mappers.OfType<IMapper<OrderItem,OrderItemDto>>().First();

            var orderItems = command.Items.Select(item => orderItemMapper.ToDomain(item)).ToList();
            Order order = new(customerMapper.ToDomain(command.Customer), orderItems) 
            { 
            
                OrderDate = DateTime.UtcNow,
                Inserted = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                State = OrderState.New
            };  
            
            await OrderRepository.SaveOrderAsync(order, cancellationToken);
            return order.Id;
        }
    }

}
