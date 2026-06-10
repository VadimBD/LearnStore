using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetSellerHandler(ISellerRepository SellerRepository, IMapper<Seller, SellerDto> Mapper) : IRequestHandler<GetSellerQuery, SellerDto?>
    {
        public async Task<SellerDto?> Handle(GetSellerQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            if (query.Id == string.Empty)
                throw new ArgumentException("SellerId cannot be empty.", nameof(query.Id));
            var seller = (await SellerRepository.GetSellersAsync(new SellerSearchCriteria() { Id = query.Id }, cancellationToken)).FirstOrDefault();
            return seller is null ? null : Mapper.ToDto(seller);
        }
    }
}

