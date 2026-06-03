using LearnStore.Application.Commands.AuthorCommands;
using LearnStore.Application.Commands.SellerCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class DeleteSellerHandler(ISellerRepository SellerRepository, IMapper<Seller, SellerDto> Mapper) : IRequestHandler<DeleteSellerCommand, SellerDto?>
    {
        public Task<SellerDto?> Handle(DeleteSellerCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));

            if (command.SellerId is null)
                throw new ArgumentException("SellerId cannot be null", nameof(command.SellerId));
            var result = await SellerRepository.DeleteSellerAsync(command.SellerId, cancellationToken);
            if (!result.Success)
                throw new InvalidOperationException(result.Message);
            var deletedSeller = await SellerRepository.GetSellerAsync(command.SellerId, cancellationToken);
            return deletedSeller is not null ? Mapper.ToDto(deletedSeller) : null;
        }
    }
}
