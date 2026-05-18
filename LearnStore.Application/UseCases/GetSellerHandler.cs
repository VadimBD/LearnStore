using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.UseCases
{
    public class GetSellerHandler : IRequestHandler<GetSellerQuery, SellerDto?>
    {
        public Task<SellerDto?> Handle(GetSellerQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<SellerDto?>(new SellerDto
            {
                Id = request.Id,
                Name = "John Doe",
                EmailAddress = "johndoe@example.com",
                PhoneNumber = "+334234656",
                AccountBalance = 1000m
            });
           }
    }
}
