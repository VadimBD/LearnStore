using LearnStore.Application.Commands.SellerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class AddToSellerBalanceHandler(ISellerRepository SellerRepository) : IRequestHandler<AddToSellerBalanceCommand, Unit>
    {
        public async Task<Unit> Handle(AddToSellerBalanceCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            var seller = await SellerRepository.GetSellerAsync(command.SellerId,cancellationToken);
            seller.AccountBalance += command.Ammaunt;
            await SellerRepository.SaveSellerAsync(seller,cancellationToken);
            return Unit.Value;
        }
    }
}
