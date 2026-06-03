using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetSellerHandler(ISellerRepository SellerRepository, IMapper<Seller, SellerDto> Mapper) : IRequestHandler<GetSellerQuery, SellerDto?>
    {
        public async Task<SellerDto?> Handle(GetSellerQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            if (request.Id == string.Empty)
                throw new ArgumentException("SellerId cannot be empty.", nameof(request.Id));
            var seller = (await SellerRepository.GetSellersAsync(new SellerSearchCriteria() { Id = request.Id }, cancellationToken)).FirstOrDefault();
            return seller is null ? null : Mapper.ToDto(seller);
        }
    }
}

