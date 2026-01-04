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
        public ICollection<OrderItem> Items { get; set; } = [];
        public Customer? Customer { get; set; }
        public OrderState State { get; set; } = OrderState.Processing;
        public ICollection<Payment> Payments { get; set; } = [];

    }

}
