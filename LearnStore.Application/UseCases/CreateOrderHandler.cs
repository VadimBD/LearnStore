using LearnStore.Application.Commands.OrderCommands;
using LearnStore.Application.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class CreateOrderHandler( IOrderRepository OrderRepository, IValidator<CreateOrderCommand> ValidationRules, IMapperRegistry mapperRegistry, IProductRepository ProductRepository) : IRequestHandler<CreateOrderCommand, Order>
    {
        public async Task<Order> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            ArgumentNullException.ThrowIfNull(command.Items, nameof(command.Items));
            ArgumentNullException.ThrowIfNull(command.Customer, nameof(command.Customer));
            await ValidationRules.ValidateAndThrowAsync(command, cancellationToken);

            var customerMapper = mapperRegistry.Get<Customer, CustomerDto>();
            var orderItemMapper = mapperRegistry.Get<OrderItem, OrderItemDto>();

            var orderItems = command.Items.Select(item => orderItemMapper.ToDomain(item)).ToList();
            orderItems.ForEach(async item => item.PriceAtOrderTime = (await GetProductPrice(item.Product?.Id ?? 0)));
            Order order = new(customerMapper.ToDomain(command.Customer), orderItems) 
            { 
            
                OrderDate = DateTime.UtcNow,
                Inserted = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                State = OrderState.New
            };  
            
            await OrderRepository.SaveOrderAsync(order, cancellationToken);
            return order;
        }

        private async Task<decimal> GetProductPrice(int productId)
        {
            var product = await ProductRepository.GetProductNoTrackingAsync(productId, CancellationToken.None);
            if (product == null)
                throw new ArgumentException($"Product with id {productId} not found.");
            return product.Price;
        }
    }

}
