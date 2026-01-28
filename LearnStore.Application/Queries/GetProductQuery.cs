using System;
using System.Collections.Generic;
using System.Text;
using MediatR;


namespace LearnStore.Application.Queries
{
    public record class GetProductQuery (int ProductId) :  IRequest<ProductDto?>;
    
}
