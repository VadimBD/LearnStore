
using LearnStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class CustomerMapper: IMapper<Customer, CustomerDto>
    {
        private IMapper<Product, ProductDto> _productMapper;
        public CustomerMapper(IEnumerable<IMapper> mappers)
        {
            _productMapper = mappers.OfType<IMapper<Product, ProductDto>>().FirstOrDefault() ?? throw new ArgumentException("Product mapper not found", nameof(mappers));
        }
        public Customer ToDomain(CustomerDto customerDto)
        {
            ArgumentNullException.ThrowIfNull(customerDto, nameof(customerDto));
            return new Customer
            {
                Id = customerDto.Id,
                Name = customerDto.Name,
                EmailAddress = customerDto.EmailAddress,
                PhoneNumber = customerDto.PhoneNumber,
                PurchasedProducts = customerDto.PurchasedProducts.Select(p => _productMapper.ToDomain(p)).ToList()

            };
        }

        public object ToDomain(object dto)
        {
           return ToDomain((CustomerDto)dto);
        }

        public CustomerDto ToDto (Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer,nameof(customer));
            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,
                PurchasedProducts = customer.PurchasedProducts.Select(p => _productMapper.ToDto(p)).ToList()
            };
        }

        public object ToDto(object domain)
        {
            return ToDto((Customer)domain);
        }

    }
}
