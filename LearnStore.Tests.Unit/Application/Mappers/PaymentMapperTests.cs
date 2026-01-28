using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Tests.Unit.Application.Mappers
{
    public class PaymentMapperTests
    {
        
        private readonly Fixture _fixture = new();

        [Fact]
        public void ToDto_WhenPaymentIsNull_ThrowsArgumentNullException()
        {
            var mapper = new PaymentMapper();
            // Act
            Action action = () => mapper.ToDto(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("payment");
        }
        [Fact]
        public void ToDto_WhenPaymentIsValid_ReturnsExpectedDto()
        {
            var mapper = new PaymentMapper();
            // Arrange
            var payment = _fixture.Create<Payment>();
            var expectedDto = new PaymentDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
            };
            // Act
            var result = mapper.ToDto(payment);
            // Assert
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public void ToDomain_WhenPaymentDtoIsNull_ThrowsArgumentNullException()
        {
            var mapper = new PaymentMapper();
            // Act
            Action action = () => mapper.ToDomain(null!);
            // Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("paymentDto");

        }

        [Fact]
        public void ToDomain_WhenPaymentDtoIsValid_ReturnsexpectedDomain()
        {
            var mapper = new PaymentMapper();
            // Arrange
            var paymentDto = _fixture.Create<PaymentDto>();
            var expectedDomain = new Payment
            {
                Id = paymentDto.Id,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
            };
            // Act
            var result = mapper.ToDomain(paymentDto);
            // Assert
            result.Should().BeEquivalentTo(expectedDomain);
        }

    }
}
