using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrderTime { get; set; }

    }
}
