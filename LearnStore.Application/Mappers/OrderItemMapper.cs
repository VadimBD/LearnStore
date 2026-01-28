using LearnStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class OrderItemMapper(IMapper<Product,ProductDto> productMapper) :IMapper<OrderItem, OrderItemDto>
    {
        public OrderItem ToDomain(OrderItemDto orderItemDto)
        {
            ArgumentNullException.ThrowIfNull(orderItemDto, nameof(orderItemDto));
            return new OrderItem
            {
                Id = orderItemDto.Id,
                Product = orderItemDto.Product != null ? productMapper.ToDomain(orderItemDto.Product) : null,
                Quantity = orderItemDto.Quantity,
                PriceAtOrderTime = orderItemDto.PriceAtOrderTime
            };
        }

        public object ToDomain(object dto)
        {
            return ToDomain((OrderItemDto)dto);
        }

        public OrderItemDto ToDto(OrderItem orderItem) 
        {
            ArgumentNullException.ThrowIfNull(orderItem,nameof(orderItem));
            return new OrderItemDto
            {
                Id = orderItem.Id,
                Product = orderItem.Product != null ? productMapper.ToDto(orderItem.Product) : null,
                Quantity = orderItem.Quantity,
                PriceAtOrderTime = orderItem.PriceAtOrderTime
            };
        }

        public object ToDto(object domain)
        {
            return ToDto((OrderItem)domain);
        }
    }
}
