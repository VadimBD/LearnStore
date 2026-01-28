using LearnStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class PaymentMapper : IMapper<Payment, PaymentDto>
    {
        public Payment ToDomain(PaymentDto paymentDto)
        {
            ArgumentNullException.ThrowIfNull(paymentDto, nameof(paymentDto));
            return new Payment
            {
                Id = paymentDto.Id,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,

            };
        }

        public object ToDomain(object dto)
        {
            return ToDomain((PaymentDto)dto);
        }

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

        public object ToDto(object domain)
        {
            return ToDto((Payment)domain);
        }
    }
}
