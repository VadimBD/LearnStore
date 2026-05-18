using LearnStore.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class SellerMapper: IMapper<Seller, SellerDto>
    {
        public Seller ToDomain(SellerDto sellerDto)
        {
            ArgumentNullException.ThrowIfNull(sellerDto, nameof(sellerDto));
            return new Seller
            {
                Id = sellerDto.Id,
                Name = sellerDto.Name,
                EmailAddress = sellerDto.EmailAddress,
                PhoneNumber = sellerDto.PhoneNumber,
                AccountBalance = sellerDto.AccountBalance
            };
        }

        public object ToDomain(object dto)
        {
            return ToDomain((SellerDto)dto);
        }

        public SellerDto ToDto(Seller seller)
        {
            ArgumentNullException.ThrowIfNull(seller, nameof(seller));
            return new SellerDto
            {
                Id = seller.Id,
                Name = seller.Name,
                EmailAddress = seller.EmailAddress,
                PhoneNumber = seller.PhoneNumber,
                AccountBalance = seller.AccountBalance
            };
        }

        public object ToDto(object domain)
        {
            return ToDto((Seller)domain);
        }
    }
}
