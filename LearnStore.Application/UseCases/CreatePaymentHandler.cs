using LearnStore.Application.Commands.PaymentCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class CreatePaymentHandler (IOrderRepository OrderRepository ,IValidator<CreatePaymentCommand> Validator) : IRequestHandler<CreatePaymentCommand, Unit>
    {
        public async Task<Unit> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            await Validator.ValidateAndThrowAsync(command, cancellationToken);

            var order =await OrderRepository.GetOrderAsync(command.OrderId, cancellationToken);

            if (order is null)
                throw new Exception("OrderNotFount");
            order.Payments.Add(new ()
            {
                Amount = command.Amount,
                Status = command.Status,
                TransactionId = command.TransactionId,
                PaymentDate = DateTime.UtcNow
            });

            await OrderRepository.SaveOrderAsync(order, cancellationToken);
            return Unit.Value;
        }
    }
}
