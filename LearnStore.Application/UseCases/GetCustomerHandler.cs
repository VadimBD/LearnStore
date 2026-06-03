using LearnStore.Application.Commands.OrderCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetCustomerHandler(ICustomerRepository CustomerRepository , IMapper<Customer, CustomerDto> Mapper) : IRequestHandler<GetCustomerQuery, CustomerDto>
    {
        public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            if(request.Id == string.Empty)
                throw new ArgumentException("CustomerId cannot be empty.", nameof(request.Id));
            var customer = (await CustomerRepository.GetCustomerAsync(new CustomerSearchCriteria() { Id = request.Id }, cancellationToken));
            return customer is null ? null : Mapper.ToDto(customer);
        }
    }
}
