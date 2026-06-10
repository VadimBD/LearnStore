using LearnStore.Domain.Entities;
using LearnStore.Domain.Enums;
using LearnStore.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken);
        Task<IEnumerable<Order>> GetOrdersAsync(OrderSearchCriteria criteria, CancellationToken cancellationToken);
        Task<Order> SaveOrderAsync(Order order, CancellationToken cancellationToken);
        Task<bool> UpdateOrderStageAsync(Guid orderId, OrderState newStage, CancellationToken cancellationToken);
        Task<Order> DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken);
        Task<Order?> GetOrderNoTrackingAsync(Guid orderId, CancellationToken cancellationToken);
        Task AddPaymentToOrderAsync(Guid orderId, Payment payment, CancellationToken cancellationToken);

    }
}
