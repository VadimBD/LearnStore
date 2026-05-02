using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.ValueObjects
{
    public record class OrderSearchCriteria
    {
        public Guid? OrderId { get; set; }
        public int? CustomerId { get; set; }
        
    }
}
