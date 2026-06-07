using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class PaymentStatusMapper : IPaymentStatusMapper
    {
        public PaymentStatus Map(string providerStatus)
        {
            return providerStatus.ToLowerInvariant() switch
            {
                "Success" => PaymentStatus.Success,
                "Failed" => PaymentStatus.Failed,
                "Pending" => PaymentStatus.Pending,
                "Cancelled" => PaymentStatus.Cancelled
            };
        }
    }
}
