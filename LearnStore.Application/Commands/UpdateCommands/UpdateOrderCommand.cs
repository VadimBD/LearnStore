using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Commands
{
    public class UpdateOrderCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
      
        public ICollection<OrderItemDto> Items { get; set; } = [];
        public CustomerDto? Customer { get; set; }
       
        public ICollection<PaymentDto> Payments { get; set; } = [];
    }
}
