using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public record GetCustomerQuery(string Id) : IRequest<CustomerDto>;
}
