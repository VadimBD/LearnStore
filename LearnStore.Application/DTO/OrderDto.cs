using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.DTO
{
    public record class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime Inserted { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItemDto> Items { get; set; } = [];
        public CustomerDto? Customer { get; set; }
        public OrderState State { get; set; } = OrderState.New;
        public ICollection<PaymentDto> Payments { get; set; } = [];
    }
}
