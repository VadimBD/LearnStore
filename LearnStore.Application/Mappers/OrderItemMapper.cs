using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class OrderItemMapper
    {
        private readonly ProductMapper _productMapper= new();
        public OrderItemDto ToDto(OrderItem orderItem) 
        {
            ArgumentNullException.ThrowIfNull(orderItem,nameof(orderItem));
            return new OrderItemDto
            {
                Id = orderItem.Id,
                Product = orderItem.Product != null ? _productMapper.ToDto(orderItem.Product) : null,
                Quantity = orderItem.Quantity,
                PriceAtOrderTime = orderItem.PriceAtOrderTime
            };
        }

        public OrderItem ToEntity(OrderItemDto orderItemDto)
        {
            ArgumentNullException.ThrowIfNull(orderItemDto, nameof(orderItemDto));
            return new OrderItem
            {
                Id = orderItemDto.Id,
                Product = orderItemDto.Product != null ? _productMapper.ToEntity(orderItemDto.Product) : null,
                Quantity = orderItemDto.Quantity,
                PriceAtOrderTime = orderItemDto.PriceAtOrderTime
            };
        }
    }
}
