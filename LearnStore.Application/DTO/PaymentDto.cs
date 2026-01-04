using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.DTO
{
    public record class PaymentDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
