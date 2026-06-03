using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Queries
{
    public class GetSellersQuery: IRequest<IEnumerable<SellerDto>>
    {
        public string SellerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal AccountBalance { get; set; } = decimal.Zero;
    }
}
