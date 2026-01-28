using LearnStore.Application.Mappers;
using LearnStore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class CreateOrderHandler (IOrderDomainService OrderDomainService, IOrderRepository OrderRepository, IValidator<CreateOrderCommand> ValidationRules) : IRequestHandler<CreateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
           throw new NotImplementedException();
        }
    }

}
