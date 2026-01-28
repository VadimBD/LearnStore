using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, OrderDto>
    {
        public Task<OrderDto> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
