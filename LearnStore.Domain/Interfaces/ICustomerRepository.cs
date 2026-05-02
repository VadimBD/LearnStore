using LearnStore.Domain.Entities;
using LearnStore.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomersAsync(CustomerSearchCriteria criteria, CancellationToken cancellationToken);
        Task  SaveCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer> DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
