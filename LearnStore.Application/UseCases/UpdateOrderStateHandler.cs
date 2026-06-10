using LearnStore.Application.Commands.OrderCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class UpdateOrderStateHandler(IOrderRepository OrderRepository) : IRequestHandler<UpdateOrderStateCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderStateCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            bool result = await OrderRepository.UpdateOrderStageAsync(command.OrderId, command.OrderState, cancellationToken);
            return Unit.Value;
        }
    }
}
