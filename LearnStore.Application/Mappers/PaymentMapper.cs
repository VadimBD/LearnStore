using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class PaymentMapper
    {
        public PaymentDto ToDto(Payment payment)
        {
            ArgumentNullException.ThrowIfNull(payment, nameof(payment));
            return new PaymentDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
       
            };
        }
        public Payment ToEntity(PaymentDto paymentDto)
        {
            ArgumentNullException.ThrowIfNull(paymentDto, nameof(paymentDto));
            return new Payment
            {
                Id = paymentDto.Id,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
            };
        }

    }
}
