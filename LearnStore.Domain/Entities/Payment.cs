using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }=string.Empty;
    }
}
