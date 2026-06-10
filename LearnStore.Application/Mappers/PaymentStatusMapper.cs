using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class PaymentStatusMapper : IPaymentStatusMapper
    {
        public PaymentStatus Map(string providerStatus)
        {
            return providerStatus.ToLowerInvariant() switch
            {
                "success" => PaymentStatus.Success,
                "failed" => PaymentStatus.Failed,
                "pending" => PaymentStatus.Pending,
                "cancelled" => PaymentStatus.Cancelled
            };
        }
    }
}
