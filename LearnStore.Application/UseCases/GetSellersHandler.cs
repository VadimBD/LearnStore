using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetSellersHandler(ISellerRepository SellerRepository, IMapper<Seller, SellerDto> Mapper) : IRequestHandler<GetSellersQuery, IEnumerable<SellerDto>>
    {
        public async Task<IEnumerable<SellerDto>> Handle(GetSellersQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            var criteria = new SellerSearchCriteria
            {
                Id = query.SellerId,
                Name = query.Name,
                EmailAddress = query.EmailAddress,
                PhoneNumber = query.PhoneNumber,
            };

            var sellers = await SellerRepository.GetSellersAsync(criteria, cancellationToken);
            return sellers.Select(s => Mapper.ToDto(s));
        }
    }
}
