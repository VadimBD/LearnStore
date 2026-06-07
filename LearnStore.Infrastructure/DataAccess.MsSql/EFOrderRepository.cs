
namespace LearnStore.Infrastructure.DataAccess.MsSql
{
    public class EFOrderRepository (AppDbContext Context): IOrderRepository
    {
        private readonly AppDbContext _context = Context ?? throw new ArgumentNullException(nameof(Context));
        
        public async Task<Order> DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(new object[] { orderId }, cancellationToken);
            if (order is not null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync(cancellationToken);
                
            }
            return order;
        }

        public async Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await _context.Orders.Include(o => o.Customer)
                                        .Include(o => o.Items).ThenInclude(i => i.Product).ThenInclude(p => p.Category)
                                        .Include(o => o.Payments)
                                        .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }
        public async Task<IEnumerable<Order>> GetOrdersAsync(OrderSearchCriteria criteria, CancellationToken cancellationToken)
        {
            var query = _context.Orders.Include(o => o.Customer)
                .Include(o => o.Items).ThenInclude(i => i.Product).ThenInclude(p => p.Category)
                .Include(o => o.Payments)
                .AsEnumerable(); 

            if (criteria.OrderId.HasValue)
                query = query.Where(o => o.Id == criteria.OrderId.Value);

            if (!string.IsNullOrWhiteSpace(criteria.CustomerId))
                query = query.Where(o => o.Customer != null && o.Customer.Id == criteria.CustomerId);

            return query.ToList();
        }

        public async Task SaveOrderAsync(Order order, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(order, nameof(order));
            ArgumentNullException.ThrowIfNull(order.Customer, nameof(order.Customer));
            ArgumentNullException.ThrowIfNull(order.Items, nameof(order.Items));
           
            ArgumentNullException.ThrowIfNull(order.Payments, nameof(order.Payments));
            
            _context.Attach(order.Customer);
            

            foreach (var item in order.Items)
            {
                _context.Attach(item.Product);
            }

            var existingOrder = await _context.Orders .Include(o => o.Customer).Include(o => o.Items).Include(o => o.Payments).FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

            if (existingOrder == null)
                _context.Orders.Add(order);
            else
            {
                _context.Entry(existingOrder).CurrentValues.SetValues(order);

                UpdateOrderPayments(existingOrder,order);
                    UpdateOrderItems(existingOrder, order);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
        private void UpdateOrderItems(Order existingOrder, Order updatedOrder)
        {
            foreach (var updatedItem in updatedOrder.Items)
            {
                var existingItem = existingOrder.Items
                    .FirstOrDefault(i => i.Id == updatedItem.Id);

                if (existingItem == null)
                    existingOrder.Items.Add(updatedItem);
                else
                {
                    _context.Entry(existingItem).CurrentValues.SetValues(updatedItem);
                    existingItem.Product = updatedItem.Product;
                }
            }
           
            var itemsToRemove = existingOrder.Items.Where(i => !updatedOrder.Items.Any(ui => ui.Id == i.Id)).ToList();

            foreach (var item in itemsToRemove)
                existingOrder.Items.Remove(item);
           
        }
        private void UpdateOrderPayments(Order existingOrder, Order updatedOrder)
        {
            foreach (var updatedPayment in updatedOrder.Payments)
            {
                var existingPayment = existingOrder.Payments .FirstOrDefault(p => p.Id == updatedPayment.Id);

                if (existingPayment == null)
                    existingOrder.Payments.Add(updatedPayment);
                else
                    _context.Entry(existingPayment).CurrentValues.SetValues(updatedPayment);
            }

            var paymentsToRemove = existingOrder.Payments.Where(p => !updatedOrder.Payments.Any(up => up.Id == p.Id)).ToList();

            foreach (var payment in paymentsToRemove)
                existingOrder.Payments.Remove(payment);
        }

        public async Task<bool> UpdateOrderStageAsync(Guid orderId, OrderState newStage, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(new object[] { orderId }, cancellationToken);

            if (order is null)
                return false;

            order.State = newStage;
           
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

       
    }
}
