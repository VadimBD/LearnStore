using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Commands.CustomerCommands
{
    public record class AddOrderProductsToCustomerCommand : IRequest<Unit>
    {
        public Guid OrderId { get; set; } = Guid.Empty;
    }
}
