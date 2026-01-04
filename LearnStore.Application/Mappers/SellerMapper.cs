using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class SellerMapper
    {

        public SellerDto ToDto(Seller seller)
        {
            ArgumentNullException.ThrowIfNull(seller, nameof(seller));
            return new SellerDto
            {
                Id = seller.Id,
                Name = seller.Name,
                EmailAddress = seller.EmailAddress,
                PhoneNumber = seller.PhoneNumber
            };
        }
        public Seller ToEntity(SellerDto sellerDto)
        {
            ArgumentNullException.ThrowIfNull(sellerDto, nameof(sellerDto));
            return new Seller
            {
                Id = sellerDto.Id,
                Name = sellerDto.Name,
                EmailAddress = sellerDto.EmailAddress,
                PhoneNumber = sellerDto.PhoneNumber
            };
        }
    }
}
