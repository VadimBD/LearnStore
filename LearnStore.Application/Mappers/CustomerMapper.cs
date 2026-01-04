using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class CustomerMapper
    {

       public CustomerDto ToDto (Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer,nameof(customer));
            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber
            };
        }

        public Customer ToEntity (CustomerDto customerDto)
          {
            ArgumentNullException.ThrowIfNull(customerDto,nameof(customerDto));
            return new Customer
                {
                 Id = customerDto.Id,
                 Name = customerDto.Name,
                 EmailAddress = customerDto.EmailAddress,
                 PhoneNumber = customerDto.PhoneNumber
                };
        }

    }
}
