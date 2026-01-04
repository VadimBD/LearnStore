using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.DTO
{
    public record class OrderItemDto
    {
        public int Id { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrderTime { get; set; }
    }
}
