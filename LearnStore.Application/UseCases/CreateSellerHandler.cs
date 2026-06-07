using LearnStore.Application.Commands.AuthorCommands;
using LearnStore.Application.Commands.SellerCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class CreateSellerHandler(ISellerRepository SellerRepository, IValidator<CreateSellerCommand> Validator) : IRequestHandler<CreateSellerCommand, Unit>
    {
        public async Task<Unit> Handle(CreateSellerCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            ArgumentException.ThrowIfNullOrEmpty(command.Name, nameof(command.Name));
            ArgumentNullException.ThrowIfNull(command.EmailAddress, nameof(command.EmailAddress));
            ArgumentNullException.ThrowIfNull(command.PhoneNumber, nameof(command.PhoneNumber));

            await Validator.ValidateAndThrowAsync(command, cancellationToken);
            var existingSeller = await SellerRepository.GetSellersAsync( new SellerSearchCriteria { EmailAddress = command.EmailAddress }, cancellationToken);
            if(existingSeller != null)
            {
                throw new InvalidOperationException("SellerEmailAlreadyExists");
            }
            var seller = new Seller
           {
               Name = command.Name,
               EmailAddress = command.EmailAddress,
               PhoneNumber = command.PhoneNumber,
               AccountBalance = command.AccountBalance
           };
            await SellerRepository.SaveSellerAsync(seller, cancellationToken);
            return Unit.Value;

        }
    
    }
}
