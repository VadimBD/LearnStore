using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime Inserted { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItem> Items { get; init; } = [];
        public Customer Customer { get; init; }
        public OrderState State { get; set; } = OrderState.New;
        public ICollection<Payment> Payments { get; set; } = [];

        public Order()
        {
        }
        public Order(Customer customer,ICollection<OrderItem> orderItems)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));
            ArgumentNullException.ThrowIfNull(orderItems, nameof(orderItems));
            if(orderItems.Count == 0)
                throw new ArgumentException("Order must have at least one item.", nameof(orderItems));
            Items = orderItems;
            Customer = customer;
        }

    }

}
